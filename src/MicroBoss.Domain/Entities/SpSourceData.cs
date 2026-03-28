namespace MicroBoss.Domain.Entities;

public class SpSourceData
{
    public string PN { get; set; } = null!;
    public string? PG { get; set; }
    public string? Class { get; set; }
    public int? TWD { get; set; }
    public string? ENDesc { get; set; }
    public string? CNDesc { get; set; }
    public string? Successor { get; set; }
    public DateTime? LastUpdateTime { get; set; }
}
