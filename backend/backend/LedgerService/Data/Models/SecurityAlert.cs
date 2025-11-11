using System;
using System.Collections.Generic;

namespace LedgerService.Data.Models;

public partial class SecurityAlert
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public Guid UserId { get; set; }

    public string Flag { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool Acknowledged { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
