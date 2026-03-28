namespace MicroBoss.Application.DTOs;

public class CustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? ShortName { get; set; }
    public string? CustomerType { get; set; }
    public string? UnitNo { get; set; }
    public string? TaxType { get; set; }
    public string? InvoiceTitle { get; set; }
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
    public string? CustomerLevel { get; set; }
    public int? SettleDay { get; set; }
    public string? PaymentMethod { get; set; }
    public int? CheckDay { get; set; }
    public string? DeliveryMethod { get; set; }
    public decimal? SettleDiscount { get; set; }
    public string? Note { get; set; }
    public decimal? AR { get; set; }
    public decimal? CL { get; set; }
    public string? SalesId { get; set; }
}

public class CreateCustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? ShortName { get; set; }
    public string? CustomerType { get; set; }
    public string? UnitNo { get; set; }
    public string? TaxType { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? Tel1 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? ContactWindow { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? PaymentMethod { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? Note { get; set; }
}

public class CustomerQueryDto
{
    public string? Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
