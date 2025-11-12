using AuthService.DTOs.Auth;
using AuthService.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")] 
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _authService.RegisterAsync(request);
                var token = await _authService.LoginAsync(new LoginRequestDto { Alias = request.Alias, Password = request.Password });
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var tokenResponse = await _authService.LoginAsync(request);
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrecta." });
            }
        }
    }
}