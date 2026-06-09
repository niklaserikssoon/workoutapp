using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using WorkoutApp.API.Data;
using WorkoutApp.API.Models;
using workoutapp_API.DTOs;

namespace workoutapp_API.services.AI
{
    public class AiPlanService : IAiPlanService
    {
        private readonly ChatClient _chatClient;
        private readonly WorkoutDbContext _context;

        public AiPlanService(IConfiguration configuration, WorkoutDbContext context)
        {
            var apiKey = configuration["Groq:ApiKey"]
                ?? throw new InvalidOperationException("Groq API key is missing.");

            var openAiClient = new OpenAIClient(
                new ApiKeyCredential(apiKey),
                new OpenAIClientOptions { Endpoint = new Uri("https://api.groq.com/openai/v1") }
            );

            _chatClient = openAiClient.GetChatClient("llama-3.3-70b-versatile");
            _context = context;
        }

        public async Task<GeneratePlanResponseDTO> GeneratePlanAsync(GeneratePlanRequestDTO request, int userId)
        {
            var equipment = string.IsNullOrWhiteSpace(request.Equipment) ? "any equipment" : request.Equipment;

            var prompt = $$"""
                You are an experienced personal trainer. Create a workout plan based on:
                - Goals: {{request.Goal}}
                - Level: {{request.FitnessLevel}}
                - Days per week: {{request.DaysPerWeek}}
                - Equipment: {{equipment}}

                Respond ONLY with valid JSON in this exact format, no extra text:
                {
                  "days": [
                    {
                      "name": "Monday - Chest",
                      "exercises": [
                        { "name": "Bench Press", "sets": 4, "reps": 8, "weight": null }
                      ]
                    }
                  ]
                }

                IMPORTANT: Detect the language of the Goals field and use that language for day names and exercise names.
                """;

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            var response = await _chatClient.CompleteChatAsync(
                [new UserChatMessage(prompt)],
                new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 800,
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                },
                cts.Token
            );

            var json = response.Value.Content[0].Text;

            var parsed = JsonSerializer.Deserialize<AIPlanJsonDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new AIPlanJsonDTO();

            var aiWorkout = new AIWorkout
            {
                UserId = userId,
                Goal = request.Goal,
                Plan = json
            };

            _context.AIWorkouts.Add(aiWorkout);
            await _context.SaveChangesAsync();

            return new GeneratePlanResponseDTO
            {
                AIWorkoutId = aiWorkout.AIWorkoutId,
                Goal = aiWorkout.Goal,
                CreatedAt = aiWorkout.CreatedAt,
                Plan = parsed.Days
            };
        }

        public async Task<GeneratePlanResponseDTO?> GetPlanAsync(int aiWorkoutId, int userId)
        {
            var aiWorkout = await _context.AIWorkouts
                .FindAsync(aiWorkoutId);

            if (aiWorkout == null || aiWorkout.UserId != userId)
                return null;

            var parsed = JsonSerializer.Deserialize<AIPlanJsonDTO>(aiWorkout.Plan, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new AIPlanJsonDTO();

            return new GeneratePlanResponseDTO
            {
                AIWorkoutId = aiWorkout.AIWorkoutId,
                Goal = aiWorkout.Goal,
                CreatedAt = aiWorkout.CreatedAt,
                Plan = parsed.Days
            };
        }

        public async Task<List<GeneratePlanResponseDTO>> GetPlansAsync(int userId)
        {
            var plans = await _context.AIWorkouts
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return plans.Select(aiWorkout =>
            {
                var parsed = JsonSerializer.Deserialize<AIPlanJsonDTO>(aiWorkout.Plan, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new AIPlanJsonDTO();

                return new GeneratePlanResponseDTO
                {
                    AIWorkoutId = aiWorkout.AIWorkoutId,
                    Goal = aiWorkout.Goal,
                    CreatedAt = aiWorkout.CreatedAt,
                    Plan = parsed.Days
                };
            }).ToList();
        }
    }
}
