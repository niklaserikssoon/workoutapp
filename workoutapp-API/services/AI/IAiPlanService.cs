using workoutapp_API.DTOs;

namespace workoutapp_API.services.AI
{
    public interface IAiPlanService
    {
        Task<GeneratePlanResponseDTO> GeneratePlanAsync(GeneratePlanRequestDTO request);
        Task<GeneratePlanResponseDTO> SavePlanAsync(SaveAIPlanDTO dto, int userId);
        Task<GeneratePlanResponseDTO?> GetPlanAsync(int aiWorkoutId, int userId);
        Task<List<GeneratePlanResponseDTO>> GetPlansAsync(int userId);
    }
}
