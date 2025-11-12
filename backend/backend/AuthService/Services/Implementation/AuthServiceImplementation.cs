using AuthService.Data.Models;
using AuthService.DTOs.Auth;
using AuthService.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services.Implementation
{
    public class AuthServiceImplementation : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthServiceImplementation(
            AuthDbContext context,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<User> RegisterAsync(RegisterRequestDto request)
        {
            var existingUserByAlias = await _context.Users
                .FirstOrDefaultAsync(u => u.Alias == request.Alias);


            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingUserByEmail = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUserByEmail != null)
                {
                    throw new InvalidOperationException($"El email '{request.Email}' ya está registrado.");
                }
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                var existingUserByPhone = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

                if (existingUserByPhone != null)
                {
                    throw new InvalidOperationException($"El teléfono '{request.PhoneNumber}' ya está registrado.");
                }
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Alias = request.Alias,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PreferredLanguage = request.PreferredLanguage,
                DemoMode = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            newUser.HashedPassword = _passwordHasher.HashPassword(newUser, request.Password);

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return newUser;
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new InvalidOperationException($"Error al registrar: {innerMessage}", ex);
            }
        }

        public async Task<TokenResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Alias == request.Alias);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrecta.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.HashedPassword,
                request.Password
            );

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrecta.");
            }

            var token = _tokenService.GenerateToken(user);

            
            return new TokenResponseDto
            {
                Token = token,
                ExpiresIn = 60
            };
        }
    }
}