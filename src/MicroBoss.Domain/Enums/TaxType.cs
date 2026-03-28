namespace MicroBoss.Domain.Enums;

/// <summary>
/// Tax type enumeration
/// </summary>
public enum TaxType
{
    /// <summary>None (無)</summary>
    None = 0,

    /// <summary>Tax included (稅內含)</summary>
    Include = 1,

    /// <summary>Tax excluded (稅外加)</summary>
    Exclude = 2
}
