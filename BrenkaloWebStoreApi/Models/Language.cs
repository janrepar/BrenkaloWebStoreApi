using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class Language
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? CodeTable { get; set; }

    public string Name { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int? Enabled { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; } = new List<CategoryTranslation>();

    public virtual ICollection<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();

    public virtual ICollection<ProductTranslation> ProductTranslations { get; set; } = new List<ProductTranslation>();
}
