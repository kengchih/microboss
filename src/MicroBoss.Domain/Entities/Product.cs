using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class Product
{
    public string ProductId { get; set; } = null!;
    public string? ProductNo { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? ProductNameExt { get; set; }
    public string? ProductName { get; set; }
    public string? ProductBarcodeName { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? InPackUnit { get; set; }
    public string? OutPackUnit { get; set; }
    public string? InPackBarcode { get; set; }
    public string? OutPackBarcode { get; set; }
    public string? ProductImage { get; set; }
    public string? ProductSpec { get; set; }
    public string? ProductSpecExt { get; set; }
    public string? ItemNote { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? CreateOperatorId { get; set; }
    public string? Manufacturer { get; set; }
    public bool? IsSuspend { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public decimal? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public string? Voltage { get; set; }
    public string? ReferenceProductId { get; set; }
    public string? DefaultStockNo { get; set; }
    public int? InPackQty { get; set; }
    public int? OutPackQty { get; set; }

    // Navigation properties
    public ProductCost? ProductCost { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
