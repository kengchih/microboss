namespace MicroBoss.Application.DTOs;

public class InventoryDto
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameExt { get; set; }
    public string? Manufacturer { get; set; }
    public string? Voltage { get; set; }
    public string? BaseBarcode { get; set; }
    public string? BaseUnit { get; set; }
    public bool? IsSuspend { get; set; }
    public string? StockNo { get; set; }
    public int SafeQty { get; set; }
    public int CurrentStockQty { get; set; }
    public int TransitQty { get; set; }
}

public class InventoryQueryDto
{
    public string? Keyword { get; set; }
    public string? StockNo { get; set; }
    public bool? UnderSafeQty { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
