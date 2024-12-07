using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class StoreMessage
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int LanguageId { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;

    public virtual User? User { get; set; }
}
