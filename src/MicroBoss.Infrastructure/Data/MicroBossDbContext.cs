using MicroBoss.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Infrastructure.Data;

public class MicroBossDbContext : IdentityDbContext<ApplicationUser>
{
    public MicroBossDbContext(DbContextOptions<MicroBossDbContext> options) : base(options) { }

    public DbSet<ComCode> ComCodes => Set<ComCode>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SupplierBank> SupplierBanks => Set<SupplierBank>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCost> ProductCosts => Set<ProductCost>();
    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();
    public DbSet<ProductPartSuit> ProductPartSuits => Set<ProductPartSuit>();
    public DbSet<ProductOrder> ProductOrders => Set<ProductOrder>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();
    public DbSet<AccountsReceivable> AccountsReceivables => Set<AccountsReceivable>();
    public DbSet<AccountsReceivableDetail> AccountsReceivableDetails => Set<AccountsReceivableDetail>();
    public DbSet<AccountsReceivablePosting> AccountsReceivablePostings => Set<AccountsReceivablePosting>();
    public DbSet<MaintenancePartLog> MaintenancePartLogs => Set<MaintenancePartLog>();
    public DbSet<RowIndex> RowIndices => Set<RowIndex>();
    public DbSet<SpSourceData> SpSourceDatas => Set<SpSourceData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // IMPORTANT: must call base for Identity tables
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroBossDbContext).Assembly);
    }
}
