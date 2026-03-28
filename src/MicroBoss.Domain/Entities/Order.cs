using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class Order
{
    public string OrderNo { get; set; } = null!;
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public string? CreateOperator { get; set; }
    public DateTime CreateTime { get; set; }
    public string? UniteNo { get; set; }
    public string? InvoiceTitle { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ContactWindow { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? MobileNo { get; set; }
    public string? Fax { get; set; }
    public string? CustomerPO { get; set; }
    public string? DeliveryCustomer { get; set; }
    public string? SettlePeriod { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? OrderNote { get; set; }
    public string? DeliveryMethod { get; set; }
    public decimal InvoiceDiscount { get; set; }
    public decimal OrderDiscount { get; set; }
    public decimal PrePayAmount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public OrderStatus Status { get; set; }
    public decimal? DiscountRate { get; set; }
    public TaxType? TaxType { get; set; }
    public InvoiceType? InvoiceType { get; set; }
    public decimal AccountReceivableAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public DateTime? ExpectedSettleDate { get; set; }
    public DateTime? ExpectedReceivableDate { get; set; }

    // Navigation properties
    public Customer? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
