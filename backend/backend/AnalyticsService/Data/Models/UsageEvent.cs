using System;
using System.Collections.Generic;

namespace AnalyticsService.Data.Models;

public partial class UsageEvent
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Screen { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string? Details { get; set; }
}
