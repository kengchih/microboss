namespace MicroBoss.Domain.Enums;

/// <summary>
/// Stock action type enumeration
/// </summary>
public enum StockActionType
{
    /// <summary>Take / Maintenance pickup (維修取件)</summary>
    Take = 0,

    /// <summary>Return / Maintenance return (維修歸還)</summary>
    Return = 1,

    /// <summary>Sale (銷貨)</summary>
    Sale = 2,

    /// <summary>Purchase / Stock in (進貨入庫)</summary>
    Purchase = 3,

    /// <summary>Adjust (調整)</summary>
    Adjust = 9
}
