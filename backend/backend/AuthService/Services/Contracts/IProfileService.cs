using BackendHackathon.DTOs.Profile;

namespace BackendHackathon.Services.Contracts
{
    public interface IProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);

        Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto request);

        Task<AccessibilityProfileDto> GetAccessibilityProfileAsync(Guid userId);

        Task<AccessibilityProfileDto> UpdateAccessibilityProfileAsync(Guid userId, AccessibilityProfileDto request);

        Task<IEnumerable<ConsentRecordDto>> GetConsentsAsync(Guid userId);

        Task<ConsentRecordDto> UpdateConsentAsync(Guid userId, UpdateConsentRequestDto request);
    }
}
