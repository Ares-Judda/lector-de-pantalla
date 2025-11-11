using System;
using System.Collections.Generic;

namespace LedgerService.Data.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public decimal Balance { get; set; }

    public string Currency { get; set; } = null!;

    public DateTime LastUpdated { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
