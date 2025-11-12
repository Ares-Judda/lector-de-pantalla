using AuthService.Data.Models;
using AuthService.DTOs.Profile;
using AuthService.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; 

namespace AuthService.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly AuthDbContext _context;

        public ProfileService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _context.Users
                .AsNoTracking() 
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDto 
                {
                    Id = u.Id,
                    Alias = u.Alias,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    PreferredLanguage = u.PreferredLanguage,
                    DemoMode = u.DemoMode
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            return user;
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(request.PreferredLanguage))
            {
                user.PreferredLanguage = request.PreferredLanguage;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetUserProfileAsync(userId);
        }

        public async Task<AccessibilityProfileDto> GetAccessibilityProfileAsync(Guid userId)
        {
            var profile = await _context.AccessibilityProfiles
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => new AccessibilityProfileDto
                {
                    Theme = p.Theme,
                    ScreenReaderMode = p.ScreenReaderMode,
                    FontScale = p.FontScale,
                    NudgingLevel = p.NudgingLevel,
                    VoiceFeedback = p.VoiceFeedback
                })
                .FirstOrDefaultAsync();

            if (profile == null)
            {
                throw new Exception("Perfil de accesibilidad no encontrado.");
            }
            return profile;
        }

        public async Task<AccessibilityProfileDto> UpdateAccessibilityProfileAsync(Guid userId, AccessibilityProfileDto request)
        {
            var profile = await _context.AccessibilityProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                throw new Exception("Perfil de accesibilidad no encontrado.");
            }

            profile.Theme = request.Theme;
            profile.ScreenReaderMode = request.ScreenReaderMode;
            profile.FontScale = request.FontScale;
            profile.NudgingLevel = request.NudgingLevel;
            profile.VoiceFeedback = request.VoiceFeedback;
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return request; 
        }


        public async Task<IEnumerable<ConsentRecordDto>> GetConsentsAsync(Guid userId)
        {
            return await _context.ConsentRecords
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => new ConsentRecordDto
                {
                    Id = c.Id,
                    Type = c.Type,
                    Granted = c.Granted,
                    Timestamp = c.Timestamp
                })
                .ToListAsync();
        }

        public async Task<ConsentRecordDto> UpdateConsentAsync(Guid userId, UpdateConsentRequestDto request)
        {
            var consent = await _context.ConsentRecords
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Type == request.Type);

            if (consent == null)
            {
                consent = new ConsentRecord
                {
                    UserId = userId,
                    Type = request.Type,
                    Granted = request.Granted,
                    Timestamp = DateTime.UtcNow
                };
                _context.ConsentRecords.Add(consent);
            }
            else
            {
                consent.Granted = request.Granted;
                consent.Timestamp = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new ConsentRecordDto
            {
                Id = consent.Id,
                Type = consent.Type,
                Granted = consent.Granted,
                Timestamp = consent.Timestamp
            };
        }
    }
}