using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class AccountsReceivableDetail
{
    public int Id { get; set; }
    public int AccountsReceivableId { get; set; }
    public AccountsReceivableType Type { get; set; }
    public string? OrderNo { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? OperatorId { get; set; }

    // Navigation properties
    public AccountsReceivable AccountsReceivable { get; set; } = null!;
}
