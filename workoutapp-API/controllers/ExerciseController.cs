using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using workoutapp_API.DTOs;
using workoutapp_API.services;

namespace workoutapp_API.controllers
{
    /// <summary>
    /// Manages exercises.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/exercises")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExternalExercise _externalExercise;

        public ExerciseController(IExternalExercise externalExercise)
        {
            _externalExercise = externalExercise;
        }

        /// <summary>
        /// Returns a paginated list of exercises.
        /// </summary>
        /// <param name="name">Optional name filter</param>
        /// <param name="level">Optional level filter (e.g. beginner, intermediate, expert)</param>
        /// <param name="muscle">Optional muscle group filter</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 20)</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        public async Task<ActionResult<PagedResult<ExternalExerciseDTO>>> GetExercises(
            [FromQuery] string? name,
            [FromQuery] string? level,
            [FromQuery] string? muscle,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var exercises = await _externalExercise.GetExercisesAsync();

            if (!string.IsNullOrWhiteSpace(name))
                exercises = exercises.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(level))
                exercises = exercises.Where(e => e.Level.Equals(level, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(muscle))
                exercises = exercises.Where(e => e.PrimaryMuscles.Any(m => m.Contains(muscle, StringComparison.OrdinalIgnoreCase))).ToList();

            var total = exercises.Count();
            var items = exercises.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new PagedResult<ExternalExerciseDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }
    }
}