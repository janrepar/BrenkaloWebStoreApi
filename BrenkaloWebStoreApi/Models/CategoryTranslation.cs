using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class CategoryTranslation
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
