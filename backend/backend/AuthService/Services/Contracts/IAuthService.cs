using AuthService.Data.Models;
using AuthService.DTOs.Auth;

namespace AuthService.Services.Contracts
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequestDto request);
        Task<TokenResponseDto> LoginAsync(LoginRequestDto request);
    }
}