namespace MicroBoss.Domain.Entities;

public class ProductStock
{
    public string StockNo { get; set; } = null!;
    public string ProductId { get; set; } = null!;
    public int? StockQty { get; set; }
    public int? SafeQty { get; set; }
    public string? Location { get; set; }
    public DateTime? InventoryDate { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public int? DeliveryQty { get; set; }
    public int? TransitQty { get; set; }
    public int? BorrowInQty { get; set; }
    public int? BorrowOutQty { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
}
