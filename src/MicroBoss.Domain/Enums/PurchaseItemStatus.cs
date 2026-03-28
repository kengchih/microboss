namespace MicroBoss.Domain.Enums;

/// <summary>
/// Purchase item status enumeration
/// </summary>
public enum PurchaseItemStatus
{
    /// <summary>Pending (待處理)</summary>
    Pending = 0,

    /// <summary>In transit (在途)</summary>
    Transit = 1,

    /// <summary>Arrived (已到貨)</summary>
    Arrived = 2
}
