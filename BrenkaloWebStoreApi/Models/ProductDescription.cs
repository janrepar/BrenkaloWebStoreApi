using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class ProductDescription
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int LanguageId { get; set; }

    public string Description { get; set; } = null!;

    public int DescriptionTypeId { get; set; }

    public virtual ProductDescriptionType DescriptionType { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
