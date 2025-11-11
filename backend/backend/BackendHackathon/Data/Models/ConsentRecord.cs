using System;
using System.Collections.Generic;

namespace BackendHackathon.Data.Models;

public partial class ConsentRecord
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Type { get; set; } = null!;

    public bool Granted { get; set; }

    public DateTime Timestamp { get; set; }

    public DateTime? RevokedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
