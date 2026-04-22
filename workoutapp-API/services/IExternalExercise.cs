using workoutapp_API.DTOs;

namespace workoutapp_API.services
{
    public interface IExternalExercise
    {
        Task<IEnumerable<ExternalExerciseDTO>> GetExercisesAsync();
        Task<ExternalExerciseDTO?> GetExerciseByIdAsync(string exerciseId);
    }
}
