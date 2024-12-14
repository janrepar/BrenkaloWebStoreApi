using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class ProductTranslation
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
