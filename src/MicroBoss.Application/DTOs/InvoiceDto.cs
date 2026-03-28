namespace MicroBoss.Application.DTOs;

public class InvoiceExportDto
{
    public string OrderNo { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public string? UniteNo { get; set; }
    public string? InvoiceTitle { get; set; }
    public int InvoiceType { get; set; }
    public string InvoiceTypeName => InvoiceType switch
    {
        2 => "二聯式",
        3 => "三聯式",
        _ => "無"
    };
    public decimal ItemAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
