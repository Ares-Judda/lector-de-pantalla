using AuthService.Data.Models;

namespace AuthService.Services.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
