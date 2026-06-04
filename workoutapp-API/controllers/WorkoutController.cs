using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using workoutapp_API.DTOs;
using workoutapp_API.services.Workouts;

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
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        /// <summary>
        /// Returns all workouts for the authenticated user.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<WorkoutDTO>>> GetWorkoutsAsync()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var workouts = await _workoutService.GetWorkoutsAsync(userId.Value);
            return Ok(workouts);
        }

        /// <summary>
        /// Returns a single workout belonging to the authenticated user.
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <response code="200">Returns the workout</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutDTO>> GetWorkoutAsync(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var workout = await _workoutService.GetWorkoutAsync(id, userId.Value);
            if (workout == null) return NotFound();

            return Ok(workout);
        }

        /// <summary>
        /// Creates a new workout with the given exercises for the authenticated user.
        /// </summary>
        /// <param name="dto">List of exercise IDs to include in the workout.</param>
        /// <response code="201">Created successfully</response>
        /// <response code="400">One or more exercises not found</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<WorkoutDTO>> CreateWorkoutAsync([FromBody] CreateWorkoutDTO dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var workout = await _workoutService.CreateWorkoutAsync(dto, userId.Value);
                return Created($"/api/v1/workouts/{workout.WorkoutId}", workout);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a workout belonging to the authenticated user.
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <response code="204">Deleted successfully</response>
        /// <response code="404">Not found</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutAsync(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _workoutService.DeleteWorkoutAsync(id, userId.Value);
            if (!deleted) return NotFound();

            return NoContent();
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}
