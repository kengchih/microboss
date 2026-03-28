using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class PurchaseDetail
{
    public string PurchaseId { get; set; } = null!;
    public string ItemId { get; set; } = null!;
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductChtName { get; set; }
    public string? ProductEngName { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Qty { get; set; }
    public decimal ItemDiscount { get; set; }
    public decimal? SubTotal { get; set; }
    public DateTime? ArriveDate { get; set; }
    public int? SortIndex { get; set; }
    public string? ItemNote { get; set; }
    public string? StockNo { get; set; }
    public string? ItemType { get; set; }
    public int OnOrderInvQty { get; set; }
    public string? PurchaseUnit { get; set; }
    public string? Barcode { get; set; }
    public PurchaseItemStatus? ItemStatus { get; set; }

    // Navigation properties
    public Purchase Purchase { get; set; } = null!;
}
