namespace MicroBoss.Domain.Entities;

public class ComCode
{
    public string GroupId { get; set; } = string.Empty;
    public string CodeId { get; set; } = string.Empty;
    public string CodeName { get; set; } = string.Empty;
    public int? SortIndex { get; set; }
    public bool? IsEnabled { get; set; }
    public DateTime? LastUpdateTime { get; set; }
}
