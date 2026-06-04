using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WorkoutApp.API.Data;
using WorkoutApp.API.Models;
using workoutapp_API.DTOs;

namespace workoutapp_API.services.Workouts
{
    public class WorkoutService : IWorkoutService
    {
        private readonly WorkoutDbContext _context;

        public WorkoutService(WorkoutDbContext context)
        {
            _context = context;
        }

        // get all workouts for a user, ordered by created date descending
        public async Task<List<WorkoutDTO>> GetWorkoutsAsync(int userId)
        {
            var workouts = await _context.Workouts
                .Include(w => w.Exercises)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();

            return workouts.Select(ToDTO).ToList();
        }

        // get workout by id, but only if it belongs to the user
        public async Task<WorkoutDTO?> GetWorkoutAsync(int id, int userId)
        {
            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.WorkoutId == id && w.UserId == userId);

            return workout == null ? null : ToDTO(workout);
        }

        // create workout by providing a list of exercise ids, and the user id
        public async Task<WorkoutDTO> CreateWorkoutAsync(CreateWorkoutDTO dto, int userId)
        {
            var exercises = await _context.Exercises
                .Where(e => dto.ExerciseIds.Contains(e.ExerciseId))
                .ToListAsync();

            if (exercises.Count != dto.ExerciseIds.Count)
                throw new KeyNotFoundException("One or more exercises not found.");

            var workout = new Workout
            {
                UserId = userId,
                Exercises = exercises
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return ToDTO(workout);
        }

        // delete workout by id, but only if it belongs to the user
        public async Task<bool> DeleteWorkoutAsync(int id, int userId)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.WorkoutId == id && w.UserId == userId);

            if (workout == null) return false;

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return true;
        }

        // update workout by id, but only if it belongs to the user. The only thing that can be updated is the list of exercises
        private static WorkoutDTO ToDTO(Workout w) => new()
        {
            WorkoutId = w.WorkoutId,
            UserId = w.UserId,
            CreatedAt = w.CreatedAt,
            Exercises = w.Exercises.Select(e => new ExerciseDTO
            {
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                PrimaryMuscle = e.PrimaryMuscle
            }).ToList()
        };
    }
}
