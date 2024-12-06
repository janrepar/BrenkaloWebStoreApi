using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class UserAddress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = null!;

    public string? State { get; set; }

    public string PostalCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int? IsDefault { get; set; }

    public int? Enabled { get; set; }

    public virtual User User { get; set; } = null!;
}
