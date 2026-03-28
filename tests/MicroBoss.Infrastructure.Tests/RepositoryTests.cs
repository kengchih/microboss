using MicroBoss.Domain.Entities;
using MicroBoss.Infrastructure.Data;
using MicroBoss.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Infrastructure.Tests;

public class RepositoryTests : IDisposable
{
    private readonly MicroBossDbContext _context;
    private readonly UnitOfWork _unitOfWork;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MicroBossDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _context = new MicroBossDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _unitOfWork = new UnitOfWork(_context);
    }

    [Fact]
    public async Task Repository_Add_And_GetById_Works()
    {
        var repo = _unitOfWork.Repository<Customer>();
        var customer = new Customer { CustomerId = "C001", CustomerName = "測試客戶" };
        repo.Add(customer);
        await _unitOfWork.SaveChangesAsync();
        var result = await repo.GetByIdAsync("C001");
        Assert.NotNull(result);
        Assert.Equal("測試客戶", result.CustomerName);
    }

    [Fact]
    public async Task Repository_Find_Returns_Matching_Entities()
    {
        var repo = _unitOfWork.Repository<Customer>();
        repo.Add(new Customer { CustomerId = "C001", CustomerName = "客戶A" });
        repo.Add(new Customer { CustomerId = "C002", CustomerName = "客戶B" });
        await _unitOfWork.SaveChangesAsync();
        var results = await repo.FindAsync(c => c.CustomerName!.Contains("A"));
        Assert.Single(results);
        Assert.Equal("C001", results[0].CustomerId);
    }

    [Fact]
    public async Task Repository_Remove_Deletes_Entity()
    {
        var repo = _unitOfWork.Repository<Customer>();
        var customer = new Customer { CustomerId = "C001", CustomerName = "刪除測試" };
        repo.Add(customer);
        await _unitOfWork.SaveChangesAsync();
        repo.Remove(customer);
        await _unitOfWork.SaveChangesAsync();
        var result = await repo.GetByIdAsync("C001");
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}
