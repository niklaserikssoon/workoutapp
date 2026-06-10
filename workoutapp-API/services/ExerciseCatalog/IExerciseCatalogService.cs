using WorkoutApp.API.Models;
using workoutapp_API.DTOs;

namespace workoutapp_API.services.Catalog
{
    public interface IExerciseCatalogService
    {
        Task<PagedResult<ExerciseCatalog>> GetExercisesAsync(string? name, string? level, string? muscle, string? equipment, int page, int pageSize);
        Task<ExerciseCatalog?> GetExerciseByIdAsync(string id);
    }
}
