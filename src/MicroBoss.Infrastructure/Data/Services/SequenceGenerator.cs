using MicroBoss.Application.Common;
using MicroBoss.Infrastructure.Data;

namespace MicroBoss.Infrastructure.Data.Services;

public class SequenceGenerator : ISequenceGenerator
{
    private readonly MicroBossDbContext _context;

    public SequenceGenerator(MicroBossDbContext context)
    {
        _context = context;
    }

    public async Task<string> GetNextOrderNoAsync()
    {
        var row = await _context.RowIndices.FindAsync("order")
            ?? throw new InvalidOperationException("Order RowIndex key not found");
        int value = int.Parse(row.NextValue!);
        string result = $"S{DateTime.Now:yyyyMMdd}{value:D5}";
        int next = value + 1;
        if (next > 99999) throw new InvalidOperationException("Order sequence overflow");
        row.NextValue = next.ToString();
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<string> GetNextPurchaseNoAsync()
    {
        var row = await _context.RowIndices.FindAsync("purchase")
            ?? throw new InvalidOperationException("Purchase RowIndex key not found");
        int value = int.Parse(row.NextValue!);
        string result = $"P{DateTime.Now:yyyyMMdd}{value:D4}";
        int next = value + 1;
        if (next > 9999) throw new InvalidOperationException("Purchase sequence overflow");
        row.NextValue = next.ToString();
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<string> GetNextSupplierIdAsync()
    {
        var row = await _context.RowIndices.FindAsync("supplier")
            ?? throw new InvalidOperationException("Supplier RowIndex key not found");
        int value = int.Parse(row.NextValue!);
        string result = $"SP0{value:D4}";
        row.NextValue = (value + 1).ToString();
        await _context.SaveChangesAsync();
        return result;
    }
}
