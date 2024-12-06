using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class ProductReview
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int Stars { get; set; }

    public string? ReviewText { get; set; }

    public string? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
