using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class ProductPicture
{
    public int Id { get; set; }

    public int? ParentPictureId { get; set; }

    public int ProductId { get; set; }

    public int LanguageId { get; set; }

    public string PictureUrl { get; set; } = null!;

    public string? Alt { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? PictureType { get; set; }

    public int? IsDefault { get; set; }

    public int? SortOrder { get; set; }

    public int? Enabled { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public virtual ICollection<ProductPicture> InverseParentPicture { get; set; } = new List<ProductPicture>();

    public virtual ProductPicture? ParentPicture { get; set; }

    public virtual Product Product { get; set; } = null!;
}
