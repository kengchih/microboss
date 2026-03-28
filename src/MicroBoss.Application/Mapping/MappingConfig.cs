using Mapster;
using MicroBoss.Application.DTOs;
using MicroBoss.Domain.Entities;

namespace MicroBoss.Application.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerDto>();
        config.NewConfig<CreateCustomerDto, Customer>()
            .Ignore(dest => dest.Orders);

        config.NewConfig<Supplier, SupplierDto>();
        config.NewConfig<SupplierBank, SupplierBankDto>();

        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.CostData, src => src.ProductCost)
            .Map(dest => dest.StocksData, src => src.ProductStocks);
        config.NewConfig<ProductCost, ProductCostDto>();
        config.NewConfig<ProductStock, ProductStockDto>();

        config.NewConfig<Order, OrderDto>();
        config.NewConfig<OrderDetail, OrderDetailDto>();

        config.NewConfig<Purchase, PurchaseDto>()
            .Map(dest => dest.Details, src => src.PurchaseDetails);
        config.NewConfig<PurchaseDetail, PurchaseItemDto>();

        config.NewConfig<AccountsReceivable, AccountsReceivableDto>();
        config.NewConfig<AccountsReceivableDetail, AccountsReceivableDetailDto>();
        config.NewConfig<AccountsReceivablePosting, AccountsReceivablePostingDto>();

        config.NewConfig<MaintenancePartLog, MaintenanceLogDto>();
        config.NewConfig<SpSourceData, SpSourceDataDto>();
        config.NewConfig<ComCode, ComCodeDto>();
    }
}
