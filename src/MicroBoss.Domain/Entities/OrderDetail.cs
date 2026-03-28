namespace MicroBoss.Domain.Entities;

public class OrderDetail
{
    public string DetailId { get; set; } = null!;
    public string OrderNo { get; set; } = null!;
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductNameExt { get; set; }
    public string? ProductBarcodeName { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? OrderUnit { get; set; }
    public int OrderQty { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal ItemAmount { get; set; }
    public decimal ItemDiscount { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? ItemNote { get; set; }

    // Navigation properties
    public Order Order { get; set; } = null!;
    public Product? Product { get; set; }
}
