using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class Vatrate
{
    public int Id { get; set; }

    public string Country { get; set; } = null!;

    public double VatRate { get; set; }

    public string Name { get; set; } = null!;

    public int? Enabled { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
