using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.DTOs.Analytics
{
    public class CreateUsageEventDto
    {
        [Required]
        public string Screen { get; set; }

        [Required]
        public string EventType { get; set; }

        public string? Details { get; set; }
    }
}
