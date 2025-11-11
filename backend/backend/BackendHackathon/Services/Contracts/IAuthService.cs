using BackendHackathon.Data.Models;
using BackendHackathon.DTOs.Auth;

namespace BackendHackathon.Services.Contracts
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequestDto request);
        Task<TokenResponseDto> LoginAsync(LoginRequestDto request);
    }
}