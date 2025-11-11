using BackendHackathon.Data.Models;
using BackendHackathon.DTOs.Auth;
using BackendHackathon.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BackendHackathon.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            AuthDbContext context,
            ITokenService tokenService,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> RegisterAsync(RegisterRequestDto request)
        {
            if (!string.IsNullOrEmpty(request.Email) && await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("El email ya está en uso.");
            }

            var newUser = new User
            {
                Alias = request.Alias,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, request.Password);
            newUser.HashedPassword = hashedPassword;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var defaultProfile = new AccessibilityProfile
            {
                UserId = newUser.Id
            };
            _context.AccessibilityProfiles.Add(defaultProfile);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Alias == request.Alias);

            if (user == null)
            {
                throw new Exception("Usuario o contraseña incorrecta.");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("Usuario o contraseña incorrecta.");
            }

            var tokenString = _tokenService.GenerateToken(user);

            return new TokenResponseDto
            {
                Token = tokenString,
                Alias = user.Alias
            };
        }
    }
}