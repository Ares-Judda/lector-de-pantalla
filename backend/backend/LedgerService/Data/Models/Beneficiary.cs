using System;
using System.Collections.Generic;

namespace LedgerService.Data.Models;

public partial class Beneficiary
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Alias { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public bool IsFavorite { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
