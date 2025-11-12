using AuthService.DTOs.Profile;
using AuthService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("ID de usuario no encontrado en el token.");
            }
            return userId;
        }

        // --- PERFIL DE USUARIO ---

        [HttpGet("me")] 
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserIdFromToken();
            var profile = await _profileService.GetUserProfileAsync(userId);
            return Ok(profile);
        }

        [HttpPut("me")] 
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            var updatedProfile = await _profileService.UpdateUserProfileAsync(userId, request);
            return Ok(updatedProfile);
        }

        // --- PERFIL DE ACCESIBILIDAD ---

        [HttpGet("accessibility")] 
        public async Task<IActionResult> GetMyAccessibilityProfile()
        {
            var userId = GetUserIdFromToken();
            var accessibilityProfile = await _profileService.GetAccessibilityProfileAsync(userId);
            return Ok(accessibilityProfile);
        }

        [HttpPut("accessibility")] 
        public async Task<IActionResult> UpdateMyAccessibilityProfile([FromBody] AccessibilityProfileDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            var updatedProfile = await _profileService.UpdateAccessibilityProfileAsync(userId, request);
            return Ok(updatedProfile);
        }

        // --- CONSENTIMIENTOS ---

        [HttpGet("consent")] 
        public async Task<IActionResult> GetMyConsents()
        {
            var userId = GetUserIdFromToken();
            var consents = await _profileService.GetConsentsAsync(userId);
            return Ok(consents);
        }

        [HttpPut("consent")] 
        public async Task<IActionResult> UpdateMyConsent([FromBody] UpdateConsentRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            var updatedConsent = await _profileService.UpdateConsentAsync(userId, request);
            
            return Ok(updatedConsent);
        }
    }
}