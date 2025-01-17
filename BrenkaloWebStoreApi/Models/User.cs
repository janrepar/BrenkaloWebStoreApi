﻿using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserGuid { get; set; }

    public string Username { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string Pwd { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? UserLanguageId { get; set; }

    public int? Enabled { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<StoreMessage> StoreMessages { get; set; } = new List<StoreMessage>();

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
