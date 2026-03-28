using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class AccountsReceivableDto
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
    public string? Note { get; set; }
    public List<AccountsReceivableDetailDto> Details { get; set; } = new();
    public List<AccountsReceivablePostingDto> Postings { get; set; } = new();
}

public class AccountsReceivableDetailDto
{
    public int Id { get; set; }
    public AccountsReceivableType Type { get; set; }
    public string? OrderNo { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

public class AccountsReceivablePostingDto
{
    public int Id { get; set; }
    public DateTime PostingDate { get; set; }
    public AccountsReceivablePostingType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Account { get; set; }
    public string? Bank { get; set; }
    public string? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public string? Note { get; set; }
}

public class CreateAccountsReceivablePostingDto
{
    public DateTime PostingDate { get; set; }
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public string? Account { get; set; }
    public string? Bank { get; set; }
    public string? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public string? Note { get; set; }
}

public class AccountsReceivableQueryDto
{
    public string? CustomerId { get; set; }
    public AccountsReceivableStatus? Status { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
