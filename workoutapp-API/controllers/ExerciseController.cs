using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using workoutapp_API.DTOs;
using workoutapp_API.services;

namespace workoutapp_API.controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExternalExercise _externalExercise;

        public ExerciseController(IExternalExercise externalExercise)
        {
            _externalExercise = externalExercise;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExternalExerciseDTO>>> GetExercises([FromQuery] string? name)
        {
            var exercises = await _externalExercise.GetExercisesAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                exercises = exercises
                    .Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Ok(exercises);
        }
    }
}