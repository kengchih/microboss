using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class AccountsReceivable
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTime SettleDate { get; set; }
    public string? CustomerId { get; set; }
    public decimal OrderItemAmount { get; set; }
    public decimal OrderTaxAmount { get; set; }
    public decimal OrderTotalAmount { get; set; }
    public decimal PriorPeriodBalanceAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public AccountsReceivableStatus Status { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? OperatorId { get; set; }
    public string? Note { get; set; }

    // Navigation properties
    public ICollection<AccountsReceivableDetail> Details { get; set; } = new List<AccountsReceivableDetail>();
    public ICollection<AccountsReceivablePosting> Postings { get; set; } = new List<AccountsReceivablePosting>();
}
