namespace MicroBoss.Application.Common;

public interface ISequenceGenerator
{
    Task<string> GetNextOrderNoAsync();
    Task<string> GetNextPurchaseNoAsync();
    Task<string> GetNextSupplierIdAsync();
}
