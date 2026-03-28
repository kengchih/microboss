using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class AccountsReceivablePosting
{
    public int Id { get; set; }
    public int AccountsReceivableId { get; set; }
    public DateTime PostingDate { get; set; }
    public AccountsReceivablePostingType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Account { get; set; }
    public string? Bank { get; set; }
    public string? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? OperatorId { get; set; }
    public string? Note { get; set; }

    // Navigation properties
    public AccountsReceivable AccountsReceivable { get; set; } = null!;
}
