using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.API.Models;
using workoutapp_API.DTOs;
using workoutapp_API.services.Catalog;

namespace workoutapp_API.controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/catalog")]
    public class ExerciseCatalogController : ControllerBase
    {
        private readonly IExerciseCatalogService _catalogService;
        private readonly IExerciseCatalogSeedService _seedService;

        public ExerciseCatalogController(IExerciseCatalogService catalogService, IExerciseCatalogSeedService seedService)
        {
            _catalogService = catalogService;
            _seedService = seedService;
        }

        /// <summary>
        /// Returns a paginated list of exercises from the catalog.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetExercises(
            [FromQuery] string? name,
            [FromQuery] string? level,
            [FromQuery] string? muscle,
            [FromQuery] string? equipment,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _catalogService.GetExercisesAsync(name, level, muscle, equipment, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Returns a single exercise from the catalog by id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExerciseById(string id)
        {
            var exercise = await _catalogService.GetExerciseByIdAsync(id);
            if (exercise == null)
                return NotFound();
            return Ok(exercise);
        }

        /// <summary>
        /// Seeds the exercise catalog from the external API. Skips exercises that already exist.
        /// </summary>
        [HttpPost("seed")]
        [Authorize]
        public async Task<IActionResult> Seed()
        {
            var count = await _seedService.SeedFromExternalApiAsync();
            return Ok(new { added = count, message = $"{count} exercises added to catalog." });
        }
    }
}
