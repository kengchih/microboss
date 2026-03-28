namespace MicroBoss.Domain.Enums;

/// <summary>
/// Product class enumeration
/// </summary>
public enum ProductClass
{
    /// <summary>Stock (存貨)</summary>
    Stock = 0,

    /// <summary>Non-stock (非存貨)</summary>
    NonStock = 1,

    /// <summary>Service / Maintenance (服務/維修性質)</summary>
    Service = 2,

    /// <summary>Labor / Hourly (工時/計時性質)</summary>
    Labor = 3
}
