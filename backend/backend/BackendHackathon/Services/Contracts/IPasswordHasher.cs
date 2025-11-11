namespace BackendHackathon.Services.Contracts
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);
    }
}
