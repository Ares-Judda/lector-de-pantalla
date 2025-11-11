using BackendHackathon.Data.Models;

namespace BackendHackathon.Services.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
