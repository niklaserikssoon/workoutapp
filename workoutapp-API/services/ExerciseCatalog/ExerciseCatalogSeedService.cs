using Microsoft.EntityFrameworkCore;
using WorkoutApp.API.Data;
using WorkoutApp.API.Models;
using workoutapp_API.services.External;

namespace workoutapp_API.services.Catalog
{
    public class ExerciseCatalogSeedService : IExerciseCatalogSeedService
    {
        private readonly WorkoutDbContext _context;
        private readonly IExternalExercise _externalExercise;

        public ExerciseCatalogSeedService(WorkoutDbContext context, IExternalExercise externalExercise)
        {
            _context = context;
            _externalExercise = externalExercise;
        }

        public async Task<int> SeedFromExternalApiAsync()
        {
            var exercises = await _externalExercise.GetExercisesAsync();

            var existingIds = await _context.ExerciseCatalog
                .Select(e => e.Id)
                .ToHashSetAsync();

            var newExercises = exercises
                .Where(dto => !existingIds.Contains(dto.Id))
                .Select(dto => new ExerciseCatalog
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Force = dto.Force,
                    Level = dto.Level,
                    Mechanic = dto.Mechanic,
                    Equipment = dto.Equipment,
                    Category = dto.Category,
                    PrimaryMuscles = dto.PrimaryMuscles,
                    SecondaryMuscles = dto.SecondaryMuscles,
                    Instructions = dto.Instructions,
                    Images = dto.Images
                })
                .ToList();

            if (newExercises.Count == 0)
                return 0;

            _context.ExerciseCatalog.AddRange(newExercises);
            await _context.SaveChangesAsync();

            return newExercises.Count;
        }
    }
}
