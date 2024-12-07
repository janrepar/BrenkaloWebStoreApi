using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class StorePicture
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int LanguageId { get; set; }

    public string PictureUrl { get; set; } = null!;

    public string? Alt { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? IsDefault { get; set; }

    public int? SortOrder { get; set; }

    public string? PictureType { get; set; }

    public int? ParentPictureId { get; set; }

    public int? Size { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StorePicture> InverseParentPicture { get; set; } = new List<StorePicture>();

    public virtual StorePicture? ParentPicture { get; set; }

    public virtual Store Store { get; set; } = null!;
}
