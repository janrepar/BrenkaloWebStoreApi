using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string OpeningHours { get; set; } = null!;

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public string? ManagerName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StoreMessage> StoreMessages { get; set; } = new List<StoreMessage>();

    public virtual ICollection<StorePicture> StorePictures { get; set; } = new List<StorePicture>();
}
