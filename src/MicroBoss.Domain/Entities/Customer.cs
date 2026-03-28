namespace MicroBoss.Domain.Entities;

public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerType { get; set; }
    public string? ShortName { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? UnitNo { get; set; }
    public string? TaxType { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? Boss { get; set; }
    public string? ContactWindow { get; set; }
    public string? AccountWindow { get; set; }
    public string? RegisterAddress { get; set; }
    public string? RegisterAddressZip { get; set; }
    public string? InvoiceAddress { get; set; }
    public string? InvoiceAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? AccountContact { get; set; }
    public string? CustomerLevel { get; set; }
    public int? SettleDay { get; set; }
    public string? PaymentMethod { get; set; }
    public int? CheckDay { get; set; }
    public string? DeliveryMethod { get; set; }
    public DateTime? FirstTransDate { get; set; }
    public DateTime? LastTransDate { get; set; }
    public decimal? SettleDiscount { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? CreateOperatorId { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? Note { get; set; }
    public decimal? AR { get; set; }
    public decimal? CL { get; set; }
    public decimal? ASR { get; set; }
    public string? SalesId { get; set; }
    public string? FactoryAddressZip { get; set; }
    public string? Contact { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
