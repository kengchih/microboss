using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class OrderDto
{
    public string OrderNo { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public TaxType? TaxType { get; set; }
    public InvoiceType? InvoiceType { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? OrderNote { get; set; }
    public OrderStatus Status { get; set; }
    public decimal OrderDiscount { get; set; }
    public decimal InvoiceDiscount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal PrePayAmount { get; set; }
    public decimal AccountReceivableAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public string? CreateOperator { get; set; }
    public DateTime CreateTime { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; } = new();
}

public class OrderDetailDto
{
    public string DetailId { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductName { get; set; }
    public string? BaseUnit { get; set; }
    public string? OrderUnit { get; set; }
    public int OrderQty { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal ItemAmount { get; set; }
    public decimal ItemDiscount { get; set; }
    public string? ItemNote { get; set; }
}

public class CreateOrderDto
{
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public int? TaxType { get; set; }
    public int? InvoiceType { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ContactWindow { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? UniteNo { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? OrderNote { get; set; }
    public decimal PrePayAmount { get; set; }
    public List<CreateOrderDetailDto> OrderDetails { get; set; } = new();
}

public class CreateOrderDetailDto
{
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductName { get; set; }
    public string? BaseUnit { get; set; }
    public string? OrderUnit { get; set; }
    public int OrderQty { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal ItemDiscount { get; set; }
    public string? ItemNote { get; set; }
}

public class OrderQueryDto
{
    public string? Keyword { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? CustomerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
