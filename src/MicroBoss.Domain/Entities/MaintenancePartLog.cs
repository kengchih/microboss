using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class MaintenancePartLog
{
    public string PartLogId { get; set; } = null!;
    public DateTime LogTime { get; set; }
    public StockActionType LogType { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductId { get; set; }
    public string? Barcode { get; set; }
    public int? Qty { get; set; }
    public string? OperatorId { get; set; }
    public string? LogUnit { get; set; }
    public DateTime? SettleDate { get; set; }
    public string? SettleNo { get; set; }
}
