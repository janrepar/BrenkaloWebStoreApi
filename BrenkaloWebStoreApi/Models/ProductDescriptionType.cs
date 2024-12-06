using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class ProductDescriptionType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public int? Enabled { get; set; }

    public virtual ICollection<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();
}
