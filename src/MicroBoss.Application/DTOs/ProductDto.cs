using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class ProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameExt { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? Manufacturer { get; set; }
    public bool? IsSuspend { get; set; }
    public string? Voltage { get; set; }
    public ProductCostDto? CostData { get; set; }
    public List<ProductStockDto> StocksData { get; set; } = new();
}

public class ProductCostDto
{
    public string? Currency { get; set; }
    public decimal? MarketPrice { get; set; }
    public decimal? UnitPriceA { get; set; }
    public decimal? UnitPriceB { get; set; }
    public decimal? UnitPriceC { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? LastInStockPrice { get; set; }
    public DateTime? LastInStockDate { get; set; }
    public string? LastInStockSupplier { get; set; }
    public string? PriorityFirstSupplier { get; set; }
}

public class ProductStockDto
{
    public string StockNo { get; set; } = string.Empty;
    public int? StockQty { get; set; }
    public int? SafeQty { get; set; }
    public string? Location { get; set; }
    public int? TransitQty { get; set; }
}

public class CreateProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameExt { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? Manufacturer { get; set; }
    public string? DefaultStockNo { get; set; }
}

public class ProductQueryDto
{
    public string? Keyword { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public bool? IsSuspend { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
