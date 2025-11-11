using System;
using System.Collections.Generic;

namespace LedgerService.Data.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid FromAccountId { get; set; }

    public Guid ToBeneficiaryId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? RiskFlags { get; set; }

    public string? SpeiReference { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account FromAccount { get; set; } = null!;

    public virtual ICollection<SecurityAlert> SecurityAlerts { get; set; } = new List<SecurityAlert>();

    public virtual Beneficiary ToBeneficiary { get; set; } = null!;
}
