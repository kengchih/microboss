namespace MicroBoss.Domain.Entities;

public class Supplier
{
    public string SupplierId { get; set; } = string.Empty;
    public string? SupplierShortName { get; set; }
    public string? SupplierFullName { get; set; }
    public string? UniteTitle { get; set; }
    public string? Area { get; set; }
    public string? SupplierType { get; set; }
    public string? TaxType { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? UniteNo { get; set; }
    public string? Boss { get; set; }
    public string? ContactWindow { get; set; }
    public string? AccountWindow { get; set; }
    public string? RegisterAddress { get; set; }
    public string? RegisterAddressZip { get; set; }
    public string? InvoiceAddress { get; set; }
    public string? InvoiceAddressZip { get; set; }
    public string? FactoryAddress { get; set; }
    public string? FactoryAddressZip { get; set; }
    public string? PaymentType { get; set; }
    public int? SettleDay { get; set; }
    public int? CheckDay { get; set; }
    public string? ReceiptPayMethod { get; set; }
    public decimal? PrepareAmount { get; set; }
    public decimal? TransQuotaAmount { get; set; }
    public DateTime? FirstTransDate { get; set; }
    public DateTime? LastTransDate { get; set; }
    public decimal? SettleDiscount { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? CreateOperatorId { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? Note { get; set; }

    public ICollection<SupplierBank> Banks { get; set; } = new List<SupplierBank>();
}
