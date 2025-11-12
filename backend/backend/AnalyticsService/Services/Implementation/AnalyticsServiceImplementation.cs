using AnalyticsService.Data.Models;
using AnalyticsService.DTOs.Analytics;
using AnalyticsService.Services.Contracts;

namespace AnalyticsService.Services.Implementation
{
    public class AnalyticsServiceImplementation : IAnalyticsService
    {
        private readonly AnalyticsDbContext _context;

        public AnalyticsServiceImplementation(AnalyticsDbContext context)
        {
            _context = context;
        }

        public async Task LogEventAsync(Guid userId, CreateUsageEventDto eventDto)
        {
            var newEvent = new UsageEvent
            {
                UserId = userId,
                Screen = eventDto.Screen,
                EventType = eventDto.EventType,
                Details = eventDto.Details,
            };

            _context.UsageEvents.Add(newEvent);
            await _context.SaveChangesAsync();

        }
    }
}
