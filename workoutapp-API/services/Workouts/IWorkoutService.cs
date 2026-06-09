using workoutapp_API.DTOs;

namespace workoutapp_API.services.Workouts
{
    public interface IWorkoutService
    {
        Task<List<WorkoutDTO>> GetWorkoutsAsync(int userId);
        Task<WorkoutDTO?> GetWorkoutAsync(int id, int userId);
        Task<WorkoutDTO> CreateWorkoutAsync(CreateWorkoutDTO dto, int userId);
        Task<bool> DeleteWorkoutAsync(int id, int userId);
        Task<bool> AddCatalogExerciseToWorkoutAsync(int workoutId, string catalogExerciseId, int userId);
    }
}
