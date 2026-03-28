namespace MicroBoss.Domain.Entities;

public class ProductCost
{
    public string ProductId { get; set; } = null!;
    public string? Currency { get; set; }
    public decimal? MarketPrice { get; set; }
    public decimal? UnitPriceA { get; set; }
    public decimal? InPackagePriceA { get; set; }
    public decimal? OutPackagePriceA { get; set; }
    public decimal? UnitPriceB { get; set; }
    public decimal? InPackagePriceB { get; set; }
    public decimal? OutPackagePriceB { get; set; }
    public decimal? UnitPriceC { get; set; }
    public decimal? InPackagePriceC { get; set; }
    public decimal? OutPackagePriceC { get; set; }
    public DateTime? LastInStockDate { get; set; }
    public string? LastInStockSupplier { get; set; }
    public decimal? LastInStockPrice { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? InPackCost { get; set; }
    public decimal? OutPackCost { get; set; }
    public decimal? CostGrossRate { get; set; }
    public string? PriorityFirstSupplier { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public decimal? PackageCost { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
}
