using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class MaintenanceLogDto
{
    public string PartLogId { get; set; } = string.Empty;
    public DateTime LogTime { get; set; }
    public StockActionType LogType { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductId { get; set; }
    public string? Barcode { get; set; }
    public int? Qty { get; set; }
    public string? OperatorId { get; set; }
    public string? LogUnit { get; set; }
}
