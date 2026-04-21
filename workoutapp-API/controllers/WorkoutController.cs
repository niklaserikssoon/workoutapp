using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.API.Data;
using workoutapp_API.DTOs;
using WorkoutApp.API.Models;

namespace workoutapp_API.controllers
{
    /// <summary>
    /// Manages workout entries.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/workouts")]
    public class WorkoutController : ControllerBase
    {
        private readonly WorkoutDbContext _context;

        public WorkoutController(WorkoutDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns paginated list of workouts for a user.
        /// </summary>
        /// <param name="userId">Filter by user ID</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 20)</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResult<WorkoutDTO>>> GetWorkoutsAsync(
            [FromQuery] int? userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = _context.Workouts
                .Include(w => w.Exercise)
                .AsQueryable();

            if (userId.HasValue)
                query = query.Where(w => w.UserId == userId.Value);

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WorkoutDTO
                {
                    WorkoutId = w.WorkoutId,
                    UserId = w.UserId,
                    ExerciseId = w.ExerciseId,
                    ExerciseName = w.Exercise.ExerciseName,
                    PrimaryMuscle = w.Exercise.PrimaryMuscle
                })
                .ToListAsync();

            return Ok(new PagedResult<WorkoutDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        /// <summary>
        /// Returns a single workout by ID.
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <response code="200">Returns the workout</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutDTO>> GetWorkoutAsync(int id)
        {
            var workout = await _context.Workouts
                .Include(w => w.Exercise)
                .FirstOrDefaultAsync(w => w.WorkoutId == id);

            if (workout == null)
                return NotFound();

            return Ok(new WorkoutDTO
            {
                WorkoutId = workout.WorkoutId,
                UserId = workout.UserId,
                ExerciseId = workout.ExerciseId,
                ExerciseName = workout.Exercise.ExerciseName,
                PrimaryMuscle = workout.Exercise.PrimaryMuscle
            });
        }

        /// <summary>
        /// Creates a new workout entry.
        /// </summary>
        /// <param name="dto">Workout details</param>
        /// <response code="201">Created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<WorkoutDTO>> CreateWorkoutAsync([FromBody] CreateWorkoutDTO dto)
        {
            var workout = new Workout
            {
                UserId = dto.UserId,
                ExerciseId = dto.ExerciseId
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkoutAsync), new { id = workout.WorkoutId }, new WorkoutDTO
            {
                WorkoutId = workout.WorkoutId,
                UserId = workout.UserId,
                ExerciseId = workout.ExerciseId
            });
        }

        /// <summary>
        /// Deletes a workout by ID.
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <response code="204">Deleted successfully</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutAsync(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound();

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
