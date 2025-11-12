using System;
using System.Collections.Generic;

namespace AnalyticsService.Data.Models;

public partial class Nudge
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Screen { get; set; } = null!;

    public string NudgeType { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? TriggerReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Accepted { get; set; }
}
