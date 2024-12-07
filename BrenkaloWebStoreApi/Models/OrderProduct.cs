using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductSku { get; set; } = null!;

    public int Quantity { get; set; }

    public double PricePerUnit { get; set; }

    public double TotalPrice { get; set; }

    public double? VatRate { get; set; }

    public double? VatAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
