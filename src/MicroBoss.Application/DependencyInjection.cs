using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MicroBoss.Application.Interfaces;
using MicroBoss.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBoss.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IComCodeService, ComCodeService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IAccountsReceivableService, AccountsReceivableService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IBoschImportService, BoschImportService>();
        return services;
    }
}
