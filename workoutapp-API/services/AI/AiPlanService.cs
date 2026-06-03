using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using workoutapp_API.DTOs;

namespace workoutapp_API.services.AI
{
    public class AiPlanService : IAiPlanService
    {
        private readonly ChatClient _chatClient;

        public AiPlanService(IConfiguration configuration)
        {
            var apiKey = configuration["Groq:ApiKey"]
                ?? throw new InvalidOperationException("Groq API key is missing.");

            var openAiClient = new OpenAIClient(
                new ApiKeyCredential(apiKey),
                new OpenAIClientOptions { Endpoint = new Uri("https://api.groq.com/openai/v1") }
            );

            _chatClient = openAiClient.GetChatClient("llama-3.3-70b-versatile");
        }

        public async Task<GeneratePlanResponseDTO> GeneratePlanAsync(GeneratePlanRequestDTO request)
        {
            var equipment = string.IsNullOrWhiteSpace(request.Equipment) ? "valfri utrustning" : request.Equipment;

            var prompt = $"""
                you're an experienced personal trainer. Create a workout plan based on the following:
                - Goals: {request.Goal}
                - Level: {request.FitnessLevel}
                - Days per week: {request.DaysPerWeek}
                - Equipment: {equipment}

                Respond with a clear weekly schedule. For each workout day, list exercises with sets and reps.
                Keep the response concise and structured. Respond in the language specified in the request.goal field.
                """;

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var response = await _chatClient.CompleteChatAsync(
                [new UserChatMessage(prompt)],
                new ChatCompletionOptions { MaxOutputTokenCount = 600 },
                cts.Token
            );

            var plan = response.Value.Content[0].Text;

            return new GeneratePlanResponseDTO { Plan = plan };
        }
    }
}
