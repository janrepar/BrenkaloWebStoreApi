using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class UserSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public string? RequestIp { get; set; }

    public DateTime ValidUntil { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
