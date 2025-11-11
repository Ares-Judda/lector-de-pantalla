using System;
using System.Collections.Generic;

namespace BackendHackathon.Data.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Alias { get; set; } = null!;

    public string PreferredLanguage { get; set; } = null!;

    public bool DemoMode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string HashedPassword { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public virtual AccessibilityProfile? AccessibilityProfile { get; set; }

    public virtual ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
