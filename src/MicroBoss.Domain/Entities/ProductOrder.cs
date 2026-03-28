namespace MicroBoss.Domain.Entities;

public class ProductOrder
{
    public string ProductId { get; set; } = null!;
    public int? DeliveryQty { get; set; }
    public int? LastOrderQty { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public int? ReOrderQty { get; set; }
    public int? ReOrderPoint { get; set; }
    public DateTime? OrderExpectedDate { get; set; }
    public int? LeadTime { get; set; }
    public DateTime? LastUpdateTime { get; set; }
}
