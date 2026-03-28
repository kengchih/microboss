using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class PurchaseDto
{
    public string PurchaseId { get; set; } = string.Empty;
    public string? SupplierId { get; set; }
    public string? SupplierShortName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public TaxType TaxType { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public PurchaseStatus? Status { get; set; }
    public string? PurchaseNote { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public DateTime? CreateTime { get; set; }
    public List<PurchaseItemDto> Details { get; set; } = new();
}

public class PurchaseItemDto
{
    public string ItemId { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductChtName { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Qty { get; set; }
    public decimal? SubTotal { get; set; }
    public PurchaseItemStatus? ItemStatus { get; set; }
    public string? StockNo { get; set; }
    public string? ItemNote { get; set; }
}

public class CreatePurchaseDto
{
    public string? SupplierId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public int TaxType { get; set; }
    public int InvoiceType { get; set; }
    public string? PurchaseNote { get; set; }
    public List<CreatePurchaseItemDto> Details { get; set; } = new();
}

public class CreatePurchaseItemDto
{
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductChtName { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Qty { get; set; }
    public string? StockNo { get; set; }
    public string? ItemNote { get; set; }
}

public class PurchaseQueryDto
{
    public string? Keyword { get; set; }
    public PurchaseStatus? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
