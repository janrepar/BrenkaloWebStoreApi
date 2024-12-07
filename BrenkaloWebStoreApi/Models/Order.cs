using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string OrderShippingMethod { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string? CustomerPhone { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string? BillingAddress { get; set; }

    public int OrderStatusId { get; set; }

    public string? CustomerNotes { get; set; }

    public double TotalAmount { get; set; }

    public double? VatAmount { get; set; }

    public double? DiscountAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? ShippingTrackingCode { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual User? User { get; set; }
}
