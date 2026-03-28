namespace MicroBoss.Domain.Enums;

/// <summary>
/// Invoice type enumeration
/// </summary>
public enum InvoiceType
{
    /// <summary>None (無)</summary>
    None = 0,

    /// <summary>Personal / Two-part (二聯式)</summary>
    Personal = 2,

    /// <summary>Business / Three-part (三聯式)</summary>
    Business = 3
}
