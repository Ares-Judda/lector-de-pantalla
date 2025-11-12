using System;
using System.Collections.Generic;

namespace OpenFinanceService.Data.Models;

public partial class OpenFinanceConnection
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string ProviderName { get; set; } = null!;

    public string Scopes { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string AuthToken { get; set; } = null!;

    public DateTime LastSync { get; set; }

    public virtual ICollection<ExternalProduct> ExternalProducts { get; set; } = new List<ExternalProduct>();
}
