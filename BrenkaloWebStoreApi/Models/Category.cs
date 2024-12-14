using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class Category
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public int? Enabled { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; } = new List<CategoryTranslation>();

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Product> ProductCategories { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductSubcategories { get; set; } = new List<Product>();
}
