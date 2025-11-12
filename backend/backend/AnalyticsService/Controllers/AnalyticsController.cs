using AnalyticsService.DTOs.Analytics;
using AnalyticsService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnalyticsController : BaseController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpPost("events")]
        public async Task<IActionResult> LogUsageEvent([FromBody] CreateUsageEventDto eventDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();

            await _analyticsService.LogEventAsync(userId, eventDto);

            return Accepted();
        }
    }
}
