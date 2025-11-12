using System;
using System.Collections.Generic;

namespace OpenFinanceService.Data.Models;

public partial class ExternalProduct
{
    public Guid Id { get; set; }

    public Guid ConnectionId { get; set; }

    public string Provider { get; set; } = null!;

    public string ProductType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Balance { get; set; }

    public string Currency { get; set; } = null!;

    public decimal? NextPaymentAmount { get; set; }

    public DateOnly? NextPaymentDate { get; set; }

    public DateTime LastSync { get; set; }

    public virtual OpenFinanceConnection Connection { get; set; } = null!;
}
