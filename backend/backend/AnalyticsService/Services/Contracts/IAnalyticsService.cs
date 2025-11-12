using AnalyticsService.DTOs.Analytics;

namespace AnalyticsService.Services.Contracts
{
    public interface IAnalyticsService
    {
        Task LogEventAsync(Guid userId, CreateUsageEventDto eventDto);
    }
}
