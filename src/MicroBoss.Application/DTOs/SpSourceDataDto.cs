namespace MicroBoss.Application.DTOs;

public class SpSourceDataDto
{
    public string PN { get; set; } = string.Empty;
    public string? PG { get; set; }
    public string? Class { get; set; }
    public int? TWD { get; set; }
    public string? ENDesc { get; set; }
    public string? CNDesc { get; set; }
    public string? Successor { get; set; }
}
