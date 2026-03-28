namespace MicroBoss.Domain.Enums;

/// <summary>
/// Order status enumeration
/// </summary>
public enum OrderStatus
{
    /// <summary>Pending (待處理)</summary>
    Pending = 0,

    /// <summary>Unpaid (未付款)</summary>
    UnPaid = 1,

    /// <summary>Closed / Paid (已付款)</summary>
    Closed = 2,

    /// <summary>Paid but unclear (已付款未結清)</summary>
    Unclear = 3,

    /// <summary>Invalid / Cancelled (已作廢)</summary>
    Invalid = 9
}
