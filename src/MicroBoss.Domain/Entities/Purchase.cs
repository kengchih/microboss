using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class Purchase
{
    public string PurchaseId { get; set; } = null!;
    public string? SupplierId { get; set; }
    public string? SupplierShortName { get; set; }
    public string? SupplierFullName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? ContactWindow { get; set; }
    public string? SupplierTel { get; set; }
    public string? SupplierFax { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? CreatedOperator { get; set; }
    public string? PurchaseNote { get; set; }
    public TaxType TaxType { get; set; }
    public string? SettleMonth { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? InvoiceDueDate { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public PurchaseStatus? Status { get; set; }

    // Navigation properties
    public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
