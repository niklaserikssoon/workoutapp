using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using workoutapp_API.DTOs;
using workoutapp_API.services.AI;

namespace workoutapp_API.controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ai")]
    public class AiController : ControllerBase
    {
        private readonly IAiPlanService _aiPlanService;

        public AiController(IAiPlanService aiPlanService)
        {
            _aiPlanService = aiPlanService;
        }

        /// <summary>
        /// Generates a personalized workout plan using AI and saves it.
        /// </summary>
        /// <summary>
        /// Generates a workout plan using AI without saving it.
        /// </summary>
        [Authorize]
        [HttpPost("generate-plan")]
        public async Task<ActionResult<GeneratePlanResponseDTO>> GeneratePlanAsync([FromBody] GeneratePlanRequestDTO request)
        {
            try
            {
                var result = await _aiPlanService.GeneratePlanAsync(request);
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(503, new { Message = "AI service timed out. Please try again." });
            }
            catch (Exception)
            {
                return StatusCode(502, new { Message = "AI service is currently unavailable. Please try again later." });
            }
        }

        /// <summary>
        /// Saves a generated AI plan to the database.
        /// </summary>
        [Authorize]
        [HttpPost("plans")]
        public async Task<ActionResult<GeneratePlanResponseDTO>> SavePlanAsync([FromBody] SaveAIPlanDTO dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _aiPlanService.SavePlanAsync(dto, userId.Value);
            return Ok(result);
        }

        /// <summary>
        /// Returns all AI-generated plans for the authenticated user.
        /// </summary>
        [Authorize]
        [HttpGet("plans")]
        public async Task<ActionResult<List<GeneratePlanResponseDTO>>> GetPlansAsync()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var plans = await _aiPlanService.GetPlansAsync(userId.Value);
            return Ok(plans);
        }

        /// <summary>
        /// Returns a single AI-generated plan by ID.
        /// </summary>
        [Authorize]
        [HttpGet("plans/{id}")]
        public async Task<ActionResult<GeneratePlanResponseDTO>> GetPlanAsync(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var plan = await _aiPlanService.GetPlanAsync(id, userId.Value);
            if (plan == null) return NotFound();

            return Ok(plan);
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}
