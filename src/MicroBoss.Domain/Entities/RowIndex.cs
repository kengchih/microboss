namespace MicroBoss.Domain.Entities;

public class RowIndex
{
    public string RowKey { get; set; } = string.Empty;
    public string? NextValue { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public byte[]? RowVersion { get; set; }
}
