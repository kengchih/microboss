namespace MicroBoss.Domain.Entities;

public class SupplierBank
{
    public string SupplierId { get; set; } = string.Empty;
    public string BankAccount { get; set; } = string.Empty;
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? BankTel { get; set; }
    public string? ItemNote { get; set; }
    public DateTime? LastUpdateTime { get; set; }

    public Supplier Supplier { get; set; } = null!;
}
