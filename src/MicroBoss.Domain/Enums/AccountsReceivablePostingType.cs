namespace MicroBoss.Domain.Enums;

/// <summary>
/// Accounts receivable posting type enumeration
/// </summary>
public enum AccountsReceivablePostingType
{
    /// <summary>Cash (現金)</summary>
    Cash = 0,

    /// <summary>Remittance (匯款)</summary>
    Remittance = 1,

    /// <summary>Check (支票)</summary>
    Check = 2,

    /// <summary>Adjustment (調帳)</summary>
    Adjustment = 8,

    /// <summary>Other (其他)</summary>
    Other = 9
}
