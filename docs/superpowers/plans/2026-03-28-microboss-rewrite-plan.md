# MicroBoss .NET 10 重寫實作計畫

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 將 MicroBoss ERP 從 .NET Framework 4.5 (WinForms + WCF + EF6) 全面重寫為 .NET 10 (Blazor Server + REST API + EF Core)，採用 Clean Architecture。

**Architecture:** Clean Architecture 四層：Domain (實體+介面) → Application (服務+DTO) → Infrastructure (EF Core+Identity) → Web (Blazor Server+API)。依賴方向由外向內，Domain 零依賴。

**Tech Stack:** .NET 10, Blazor Server, EF Core, ASP.NET Core Identity, Serilog, FluentValidation, Mapster, xUnit, Moq

---

## 階段總覽

本計畫分為 6 個階段，每階段可獨立完成並產出可建置的程式碼：

| 階段 | 內容 | Tasks |
|------|------|-------|
| Phase 1 | 基礎建設：Domain 層 + Infrastructure 層 + 橫切關注點 | Task 1-6 |
| Phase 2 | 認證與授權：Identity + 登入 UI | Task 7-9 |
| Phase 3 | 核心業務 - 基礎管理：ComCode + Customer + Supplier + Product | Task 10-17 |
| Phase 4 | 核心業務 - 交易流程：Order + Purchase + Inventory | Task 18-24 |
| Phase 5 | 財務模組：AccountsReceivable + Invoice | Task 25-28 |
| Phase 6 | 報表列印 + 資料匯入 + 收尾 | Task 29-32 |

## 檔案結構

### Domain 層 (`src/MicroBoss.Domain/`)
```
Entities/
  ComCode.cs, Customer.cs, Supplier.cs, SupplierBank.cs,
  Product.cs, ProductCost.cs, ProductStock.cs, ProductPartSuit.cs, ProductOrder.cs,
  Order.cs, OrderDetail.cs,
  Purchase.cs, PurchaseDetail.cs,
  AccountsReceivable.cs, AccountsReceivableDetail.cs, AccountsReceivablePosting.cs,
  MaintenancePartLog.cs, RowIndex.cs, SpSourceData.cs
Enums/
  OrderStatus.cs, TaxType.cs, InvoiceType.cs, PurchaseStatus.cs,
  PurchaseItemStatus.cs, StockActionType.cs, ProductClass.cs,
  DataStatus.cs, ComGroup.cs, AccountsReceivableStatus.cs,
  AccountsReceivableType.cs, AccountsReceivablePostingType.cs
Interfaces/
  IRepository.cs, IUnitOfWork.cs
```

### Application 層 (`src/MicroBoss.Application/`)
```
DependencyInjection.cs
Common/
  PagedResult.cs, ISequenceGenerator.cs
DTOs/
  ComCodeDto.cs, CustomerDto.cs, SupplierDto.cs, SupplierBankDto.cs,
  ProductDto.cs, ProductCostDto.cs, ProductStockDto.cs,
  OrderDto.cs, OrderDetailDto.cs,
  PurchaseDto.cs, PurchaseItemDto.cs,
  AccountsReceivableDto.cs, InventoryDto.cs,
  MaintenanceLogDto.cs, SpSourceDataDto.cs
Interfaces/
  IComCodeService.cs, ICustomerService.cs, ISupplierService.cs,
  IProductService.cs, IOrderService.cs, IPurchaseService.cs,
  IInventoryService.cs, IAccountsReceivableService.cs,
  IInvoiceService.cs, IBoschImportService.cs
Services/
  ComCodeService.cs, CustomerService.cs, SupplierService.cs,
  ProductService.cs, OrderService.cs, PurchaseService.cs,
  InventoryService.cs, AccountsReceivableService.cs,
  InvoiceService.cs, BoschImportService.cs
Mapping/
  MappingConfig.cs
Validators/
  CreateOrderValidator.cs, CreateCustomerValidator.cs,
  CreateProductValidator.cs, CreatePurchaseValidator.cs
```

### Infrastructure 層 (`src/MicroBoss.Infrastructure/`)
```
DependencyInjection.cs
Data/
  MicroBossDbContext.cs
  Configurations/
    ComCodeConfiguration.cs, CustomerConfiguration.cs, SupplierConfiguration.cs,
    SupplierBankConfiguration.cs, ProductConfiguration.cs, ProductCostConfiguration.cs,
    ProductStockConfiguration.cs, OrderConfiguration.cs, OrderDetailConfiguration.cs,
    PurchaseConfiguration.cs, PurchaseDetailConfiguration.cs,
    AccountsReceivableConfiguration.cs, AccountsReceivableDetailConfiguration.cs,
    AccountsReceivablePostingConfiguration.cs, MaintenancePartLogConfiguration.cs,
    RowIndexConfiguration.cs, SpSourceDataConfiguration.cs,
    ProductPartSuitConfiguration.cs, ProductOrderConfiguration.cs
  Repositories/
    Repository.cs, UnitOfWork.cs
  Services/
    SequenceGenerator.cs
Identity/
  IdentitySetup.cs
```

### Web 層 (`src/MicroBoss.Web/`)
```
Program.cs (修改)
Middleware/
  GlobalExceptionMiddleware.cs
Controllers/
  ComCodesController.cs, CustomersController.cs, SuppliersController.cs,
  ProductsController.cs, OrdersController.cs, PurchasesController.cs,
  InventoryController.cs, AccountsReceivableController.cs,
  InvoicesController.cs, ImportController.cs, UsersController.cs
Components/
  Layout/
    MainLayout.razor (修改), NavMenu.razor (修改)
  Pages/
    Auth/
      Login.razor
    Admin/
      Users.razor, UserEdit.razor, ComCodes.razor, ComCodeEdit.razor
    Customers/
      Customers.razor, CustomerEdit.razor
    Suppliers/
      Suppliers.razor, SupplierEdit.razor
    Products/
      Products.razor, ProductEdit.razor
    Orders/
      Orders.razor, OrderEdit.razor
    Purchases/
      Purchases.razor, PurchaseEdit.razor
    Inventory/
      Inventory.razor, SafeStock.razor
    AccountsReceivable/
      AccountsReceivable.razor, AREdit.razor, ARPosting.razor
    Pos/
      PosOrder.razor
    Import/
      BoschImport.razor, DataImport.razor
    Invoices/
      InvoiceExport.razor
    Maintenance/
      MaintenanceLogs.razor
  Shared/
    ConfirmDialog.razor
```

---

## Phase 1: 基礎建設

### Task 1: Domain 層 - Enums

**Files:**
- Create: `src/MicroBoss.Domain/Enums/OrderStatus.cs`
- Create: `src/MicroBoss.Domain/Enums/TaxType.cs`
- Create: `src/MicroBoss.Domain/Enums/InvoiceType.cs`
- Create: `src/MicroBoss.Domain/Enums/PurchaseStatus.cs`
- Create: `src/MicroBoss.Domain/Enums/PurchaseItemStatus.cs`
- Create: `src/MicroBoss.Domain/Enums/StockActionType.cs`
- Create: `src/MicroBoss.Domain/Enums/ProductClass.cs`
- Create: `src/MicroBoss.Domain/Enums/DataStatus.cs`
- Create: `src/MicroBoss.Domain/Enums/ComGroup.cs`
- Create: `src/MicroBoss.Domain/Enums/AccountsReceivableStatus.cs`
- Create: `src/MicroBoss.Domain/Enums/AccountsReceivableType.cs`
- Create: `src/MicroBoss.Domain/Enums/AccountsReceivablePostingType.cs`

- [ ] **Step 1: Create all enum files**

```csharp
// src/MicroBoss.Domain/Enums/OrderStatus.cs
namespace MicroBoss.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,    // 待處理
    UnPaid = 1,     // 未付款
    Closed = 2,     // 已付款
    Unclear = 3,    // 已付款未結清
    Invalid = 9     // 已作廢
}
```

```csharp
// src/MicroBoss.Domain/Enums/TaxType.cs
namespace MicroBoss.Domain.Enums;

public enum TaxType
{
    None = 0,       // 無
    Include = 1,    // 稅內含
    Exclude = 2     // 稅外加
}
```

```csharp
// src/MicroBoss.Domain/Enums/InvoiceType.cs
namespace MicroBoss.Domain.Enums;

public enum InvoiceType
{
    None = 0,       // 無
    Personal = 2,   // 二聯式
    Business = 3    // 三聯式
}
```

```csharp
// src/MicroBoss.Domain/Enums/PurchaseStatus.cs
namespace MicroBoss.Domain.Enums;

public enum PurchaseStatus
{
    Pending = 0,    // 待處理
    Purchased = 1   // 已採購
}
```

```csharp
// src/MicroBoss.Domain/Enums/PurchaseItemStatus.cs
namespace MicroBoss.Domain.Enums;

public enum PurchaseItemStatus
{
    Pending = 0,        // 待處理
    Transit = 1,        // 在途
    Arrived = 2         // 已到貨
}
```

```csharp
// src/MicroBoss.Domain/Enums/StockActionType.cs
namespace MicroBoss.Domain.Enums;

public enum StockActionType
{
    Take = 0,       // 維修取件
    Return = 1,     // 維修歸還
    Sale = 2,       // 銷貨
    Purchase = 3,   // 進貨入庫
    Adjust = 9      // 調整
}
```

```csharp
// src/MicroBoss.Domain/Enums/ProductClass.cs
namespace MicroBoss.Domain.Enums;

public enum ProductClass
{
    Stock = 0,      // 存貨
    NonStock = 1,   // 非存貨
    Service = 2,    // 服務/維修性質
    Labor = 3       // 工時/計時性質
}
```

```csharp
// src/MicroBoss.Domain/Enums/DataStatus.cs
namespace MicroBoss.Domain.Enums;

public enum DataStatus
{
    Added = 0,
    Updated = 1,
    UnModified = 2,
    Deleted = 3
}
```

```csharp
// src/MicroBoss.Domain/Enums/ComGroup.cs
namespace MicroBoss.Domain.Enums;

public static class ComGroup
{
    public const string Manufactory = "MF";
    public const string Unit = "UN";
    public const string Currency = "CU";
    public const string SettleMethod = "SM";
    public const string Stock = "STOCK";
    public const string Finance = "FN";
    public const string Valuation = "VL";
    public const string SupplierType = "ST";
    public const string PaymentType = "PT";
    public const string ReceiptPayMethod = "RPM";
    public const string ProductMajorCategory = "PMC";
    public const string ProductSubCategory = "PSC";
    public const string Voltage = "VOL";
    public const string CustomerType = "CSTYPE";
    public const string CustomerTaxType = "CSTXTYPE";
    public const string CustomerPaymentMethod = "CSPM";
    public const string DeliveryMethod = "DLVM";
    public const string PurchaseItemType = "PUITEMTYPE";
}
```

```csharp
// src/MicroBoss.Domain/Enums/AccountsReceivableStatus.cs
namespace MicroBoss.Domain.Enums;

public enum AccountsReceivableStatus
{
    Unpaid = 0,     // 未結帳
    Paid = 1        // 已結帳
}
```

```csharp
// src/MicroBoss.Domain/Enums/AccountsReceivableType.cs
namespace MicroBoss.Domain.Enums;

public enum AccountsReceivableType
{
    Order = 0,      // 銷貨
    Other = 9       // 其他
}
```

```csharp
// src/MicroBoss.Domain/Enums/AccountsReceivablePostingType.cs
namespace MicroBoss.Domain.Enums;

public enum AccountsReceivablePostingType
{
    Cash = 0,           // 現金
    Remittance = 1,     // 匯款
    Check = 2,          // 支票
    Adjustment = 8,     // 調帳
    Other = 9           // 其他
}
```

- [ ] **Step 2: Build to verify**

Run: `cd /mnt/c/development/repos/microboss-new && dotnet build src/MicroBoss.Domain/MicroBoss.Domain.csproj`
Expected: Build succeeded, 0 errors

- [ ] **Step 3: Commit**

```bash
cd /mnt/c/development/repos/microboss-new
git add src/MicroBoss.Domain/Enums/
git commit -m "feat(domain): add all business enum definitions"
```

---

### Task 2: Domain 層 - Entities (基礎)

**Files:**
- Create: `src/MicroBoss.Domain/Entities/ComCode.cs`
- Create: `src/MicroBoss.Domain/Entities/Customer.cs`
- Create: `src/MicroBoss.Domain/Entities/Supplier.cs`
- Create: `src/MicroBoss.Domain/Entities/SupplierBank.cs`
- Create: `src/MicroBoss.Domain/Entities/RowIndex.cs`

- [ ] **Step 1: Create ComCode entity**

```csharp
// src/MicroBoss.Domain/Entities/ComCode.cs
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
```

- [ ] **Step 2: Create Customer entity**

```csharp
// src/MicroBoss.Domain/Entities/Customer.cs
namespace MicroBoss.Domain.Entities;

public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerType { get; set; }
    public string? ShortName { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? UnitNo { get; set; }
    public string? TaxType { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? Boss { get; set; }
    public string? ContactWindow { get; set; }
    public string? AccountWindow { get; set; }
    public string? RegisterAddress { get; set; }
    public string? RegisterAddressZip { get; set; }
    public string? InvoiceAddress { get; set; }
    public string? InvoiceAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? AccountContact { get; set; }
    public string? CustomerLevel { get; set; }
    public int? SettleDay { get; set; }
    public string? PaymentMethod { get; set; }
    public int? CheckDay { get; set; }
    public string? DeliveryMethod { get; set; }
    public DateTime? FirstTransDate { get; set; }
    public DateTime? LastTransDate { get; set; }
    public decimal? SettleDiscount { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? CreateOperatorId { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? Note { get; set; }
    public decimal? AR { get; set; }
    public decimal? CL { get; set; }
    public decimal? ASR { get; set; }
    public string? SalesId { get; set; }
    public string? FactoryAddressZip { get; set; }
    public string? Contact { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
```

- [ ] **Step 3: Create Supplier and SupplierBank entities**

```csharp
// src/MicroBoss.Domain/Entities/Supplier.cs
namespace MicroBoss.Domain.Entities;

public class Supplier
{
    public string SupplierId { get; set; } = string.Empty;
    public string? SupplierShortName { get; set; }
    public string? SupplierFullName { get; set; }
    public string? UniteTitle { get; set; }
    public string? Area { get; set; }
    public string? SupplierType { get; set; }
    public string? TaxType { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? UniteNo { get; set; }
    public string? Boss { get; set; }
    public string? ContactWindow { get; set; }
    public string? AccountWindow { get; set; }
    public string? RegisterAddress { get; set; }
    public string? RegisterAddressZip { get; set; }
    public string? InvoiceAddress { get; set; }
    public string? InvoiceAddressZip { get; set; }
    public string? FactoryAddress { get; set; }
    public string? FactoryAddressZip { get; set; }
    public string? PaymentType { get; set; }
    public int? SettleDay { get; set; }
    public int? CheckDay { get; set; }
    public string? ReceiptPayMethod { get; set; }
    public decimal? PrepareAmount { get; set; }
    public decimal? TransQuotaAmount { get; set; }
    public DateTime? FirstTransDate { get; set; }
    public DateTime? LastTransDate { get; set; }
    public decimal? SettleDiscount { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? CreateOperatorId { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? Note { get; set; }

    public ICollection<SupplierBank> Banks { get; set; } = new List<SupplierBank>();
}
```

```csharp
// src/MicroBoss.Domain/Entities/SupplierBank.cs
namespace MicroBoss.Domain.Entities;

public class SupplierBank
{
    public string SupplierId { get; set; } = string.Empty;
    public string BankAccount { get; set; } = string.Empty;
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? BankTel { get; set; }
    public string? ItemNote { get; set; }
    public DateTime? LastUpdateTime { get; set; }

    public Supplier Supplier { get; set; } = null!;
}
```

- [ ] **Step 4: Create RowIndex entity**

```csharp
// src/MicroBoss.Domain/Entities/RowIndex.cs
namespace MicroBoss.Domain.Entities;

public class RowIndex
{
    public string RowKey { get; set; } = string.Empty;
    public string? NextValue { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public byte[]? RowVersion { get; set; }
}
```

- [ ] **Step 5: Build to verify**

Run: `cd /mnt/c/development/repos/microboss-new && dotnet build src/MicroBoss.Domain/MicroBoss.Domain.csproj`
Expected: Build succeeded, 0 errors

- [ ] **Step 6: Commit**

```bash
cd /mnt/c/development/repos/microboss-new
git add src/MicroBoss.Domain/Entities/
git commit -m "feat(domain): add base entities - ComCode, Customer, Supplier, RowIndex"
```

---

### Task 3: Domain 層 - Entities (產品+訂單+採購+應收)

**Files:**
- Create: `src/MicroBoss.Domain/Entities/Product.cs`
- Create: `src/MicroBoss.Domain/Entities/ProductCost.cs`
- Create: `src/MicroBoss.Domain/Entities/ProductStock.cs`
- Create: `src/MicroBoss.Domain/Entities/ProductPartSuit.cs`
- Create: `src/MicroBoss.Domain/Entities/ProductOrder.cs`
- Create: `src/MicroBoss.Domain/Entities/Order.cs`
- Create: `src/MicroBoss.Domain/Entities/OrderDetail.cs`
- Create: `src/MicroBoss.Domain/Entities/Purchase.cs`
- Create: `src/MicroBoss.Domain/Entities/PurchaseDetail.cs`
- Create: `src/MicroBoss.Domain/Entities/AccountsReceivable.cs`
- Create: `src/MicroBoss.Domain/Entities/AccountsReceivableDetail.cs`
- Create: `src/MicroBoss.Domain/Entities/AccountsReceivablePosting.cs`
- Create: `src/MicroBoss.Domain/Entities/MaintenancePartLog.cs`
- Create: `src/MicroBoss.Domain/Entities/SpSourceData.cs`

- [ ] **Step 1: Create Product-related entities**

```csharp
// src/MicroBoss.Domain/Entities/Product.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class Product
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? ProductNameExt { get; set; }
    public string? ProductName { get; set; }
    public string? ProductBarcodeName { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? InPackUnit { get; set; }
    public string? OutPackUnit { get; set; }
    public string? InPackBarcode { get; set; }
    public string? OutPackBarcode { get; set; }
    public string? ProductImage { get; set; }
    public string? ProductSpec { get; set; }
    public string? ProductSpecExt { get; set; }
    public string? ItemNote { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? CreateOperatorId { get; set; }
    public string? Manufacturer { get; set; }
    public bool? IsSuspend { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public decimal? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public string? Voltage { get; set; }
    public string? ReferenceProductId { get; set; }
    public string? DefaultStockNo { get; set; }
    public int? InPackQty { get; set; }
    public int? OutPackQty { get; set; }

    public ProductCost? ProductCost { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
```

```csharp
// src/MicroBoss.Domain/Entities/ProductCost.cs
namespace MicroBoss.Domain.Entities;

public class ProductCost
{
    public string ProductId { get; set; } = string.Empty;
    public string? Currency { get; set; }
    public decimal? MarketPrice { get; set; }
    public decimal? UnitPriceA { get; set; }
    public decimal? InPackagePriceA { get; set; }
    public decimal? OutPackagePriceA { get; set; }
    public decimal? UnitPriceB { get; set; }
    public decimal? InPackagePriceB { get; set; }
    public decimal? OutPackagePriceB { get; set; }
    public decimal? UnitPriceC { get; set; }
    public decimal? InPackagePriceC { get; set; }
    public decimal? OutPackagePriceC { get; set; }
    public DateTime? LastInStockDate { get; set; }
    public string? LastInStockSupplier { get; set; }
    public decimal? LastInStockPrice { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? InPackCost { get; set; }
    public decimal? OutPackCost { get; set; }
    public decimal? CostGrossRate { get; set; }
    public string? PriorityFirstSupplier { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public decimal? PackageCost { get; set; }

    public Product Product { get; set; } = null!;
}
```

```csharp
// src/MicroBoss.Domain/Entities/ProductStock.cs
namespace MicroBoss.Domain.Entities;

public class ProductStock
{
    public string StockNo { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public int? StockQty { get; set; }
    public int? SafeQty { get; set; }
    public string? Location { get; set; }
    public DateTime? InventoryDate { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public int? DeliveryQty { get; set; }
    public int? TransitQty { get; set; }
    public int? BorrowInQty { get; set; }
    public int? BorrowOutQty { get; set; }

    public Product Product { get; set; } = null!;
}
```

```csharp
// src/MicroBoss.Domain/Entities/ProductPartSuit.cs
namespace MicroBoss.Domain.Entities;

public class ProductPartSuit
{
    public int SuitId { get; set; }
    public string? ReferenceProductId { get; set; }
}
```

```csharp
// src/MicroBoss.Domain/Entities/ProductOrder.cs
namespace MicroBoss.Domain.Entities;

public class ProductOrder
{
    public string ProductId { get; set; } = string.Empty;
    public int? DeliveryQty { get; set; }
    public int? LastOrderQty { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public int? ReOrderQty { get; set; }
    public int? ReOrderPoint { get; set; }
    public DateTime? OrderExpectedDate { get; set; }
    public int? LeadTime { get; set; }
    public DateTime? LastUpdateTime { get; set; }
}
```

- [ ] **Step 2: Create Order entities**

```csharp
// src/MicroBoss.Domain/Entities/Order.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class Order
{
    public string OrderNo { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public string? CreateOperator { get; set; }
    public DateTime CreateTime { get; set; }
    public string? UniteNo { get; set; }
    public string? InvoiceTitle { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ContactWindow { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? MobileNo { get; set; }
    public string? Fax { get; set; }
    public string? CustomerPO { get; set; }
    public string? DeliveryCustomer { get; set; }
    public string? SettlePeriod { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? OrderNote { get; set; }
    public string? DeliveryMethod { get; set; }
    public decimal InvoiceDiscount { get; set; }
    public decimal OrderDiscount { get; set; }
    public decimal PrePayAmount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public OrderStatus Status { get; set; }
    public decimal? DiscountRate { get; set; }
    public TaxType? TaxType { get; set; }
    public InvoiceType? InvoiceType { get; set; }
    public decimal AccountReceivableAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public DateTime? ExpectedSettleDate { get; set; }
    public DateTime? ExpectedReceivableDate { get; set; }

    public Customer? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
```

```csharp
// src/MicroBoss.Domain/Entities/OrderDetail.cs
namespace MicroBoss.Domain.Entities;

public class OrderDetail
{
    public string DetailId { get; set; } = string.Empty;
    public string OrderNo { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductNameExt { get; set; }
    public string? ProductBarcodeName { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? OrderUnit { get; set; }
    public int OrderQty { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal ItemAmount { get; set; }
    public decimal ItemDiscount { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string? ItemNote { get; set; }

    public Order Order { get; set; } = null!;
    public Product? Product { get; set; }
}
```

- [ ] **Step 3: Create Purchase entities**

```csharp
// src/MicroBoss.Domain/Entities/Purchase.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class Purchase
{
    public string PurchaseId { get; set; } = string.Empty;
    public string? SupplierId { get; set; }
    public string? SupplierShortName { get; set; }
    public string? SupplierFullName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? ContactWindow { get; set; }
    public string? SupplierTel { get; set; }
    public string? SupplierFax { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? CreatedOperator { get; set; }
    public string? PurchaseNote { get; set; }
    public TaxType TaxType { get; set; }
    public string? SettleMonth { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? InvoiceDueDate { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public PurchaseStatus? Status { get; set; }

    public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
```

```csharp
// src/MicroBoss.Domain/Entities/PurchaseDetail.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class PurchaseDetail
{
    public string PurchaseId { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductChtName { get; set; }
    public string? ProductEngName { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Qty { get; set; }
    public decimal ItemDiscount { get; set; }
    public decimal? SubTotal { get; set; }
    public DateTime? ArriveDate { get; set; }
    public int? SortIndex { get; set; }
    public string? ItemNote { get; set; }
    public string? StockNo { get; set; }
    public string? ItemType { get; set; }
    public int OnOrderInvQty { get; set; }
    public string? PurchaseUnit { get; set; }
    public string? Barcode { get; set; }
    public PurchaseItemStatus? ItemStatus { get; set; }

    public Purchase Purchase { get; set; } = null!;
}
```

- [ ] **Step 4: Create AccountsReceivable entities**

```csharp
// src/MicroBoss.Domain/Entities/AccountsReceivable.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class AccountsReceivable
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTime SettleDate { get; set; }
    public string? CustomerId { get; set; }
    public decimal OrderItemAmount { get; set; }
    public decimal OrderTaxAmount { get; set; }
    public decimal OrderTotalAmount { get; set; }
    public decimal PriorPeriodBalanceAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public AccountsReceivableStatus Status { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? OperatorId { get; set; }
    public string? Note { get; set; }

    public ICollection<AccountsReceivableDetail> Details { get; set; } = new List<AccountsReceivableDetail>();
    public ICollection<AccountsReceivablePosting> Postings { get; set; } = new List<AccountsReceivablePosting>();
}
```

```csharp
// src/MicroBoss.Domain/Entities/AccountsReceivableDetail.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class AccountsReceivableDetail
{
    public int Id { get; set; }
    public int AccountsReceivableId { get; set; }
    public AccountsReceivableType Type { get; set; }
    public string? OrderNo { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? OperatorId { get; set; }

    public AccountsReceivable AccountsReceivable { get; set; } = null!;
}
```

```csharp
// src/MicroBoss.Domain/Entities/AccountsReceivablePosting.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class AccountsReceivablePosting
{
    public int Id { get; set; }
    public int AccountsReceivableId { get; set; }
    public DateTime PostingDate { get; set; }
    public AccountsReceivablePostingType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Account { get; set; }
    public string? Bank { get; set; }
    public string? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? OperatorId { get; set; }
    public string? Note { get; set; }

    public AccountsReceivable AccountsReceivable { get; set; } = null!;
}
```

- [ ] **Step 5: Create remaining entities**

```csharp
// src/MicroBoss.Domain/Entities/MaintenancePartLog.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Domain.Entities;

public class MaintenancePartLog
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
    public DateTime? SettleDate { get; set; }
    public string? SettleNo { get; set; }
}
```

```csharp
// src/MicroBoss.Domain/Entities/SpSourceData.cs
namespace MicroBoss.Domain.Entities;

public class SpSourceData
{
    public string PN { get; set; } = string.Empty;
    public string? PG { get; set; }
    public string? Class { get; set; }
    public int? TWD { get; set; }
    public string? ENDesc { get; set; }
    public string? CNDesc { get; set; }
    public string? Successor { get; set; }
    public DateTime? LastUpdateTime { get; set; }
}
```

- [ ] **Step 6: Build to verify**

Run: `cd /mnt/c/development/repos/microboss-new && dotnet build src/MicroBoss.Domain/MicroBoss.Domain.csproj`
Expected: Build succeeded, 0 errors

- [ ] **Step 7: Commit**

```bash
cd /mnt/c/development/repos/microboss-new
git add src/MicroBoss.Domain/Entities/
git commit -m "feat(domain): add all business entities"
```

---

### Task 4: Domain 層 - Repository 介面 + Infrastructure 層 - EF Core 設定

**Files:**
- Create: `src/MicroBoss.Domain/Interfaces/IRepository.cs`
- Create: `src/MicroBoss.Domain/Interfaces/IUnitOfWork.cs`
- Create: `src/MicroBoss.Infrastructure/Data/MicroBossDbContext.cs`
- Create: `src/MicroBoss.Infrastructure/Data/Repositories/Repository.cs`
- Create: `src/MicroBoss.Infrastructure/Data/Repositories/UnitOfWork.cs`
- Modify: `src/MicroBoss.Infrastructure/MicroBoss.Infrastructure.csproj`
- Test: `tests/MicroBoss.Infrastructure.Tests/RepositoryTests.cs`

- [ ] **Step 1: Add EF Core NuGet packages to Infrastructure**

Run:
```bash
cd /mnt/c/development/repos/microboss-new
dotnet add src/MicroBoss.Infrastructure/MicroBoss.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/MicroBoss.Infrastructure/MicroBoss.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add tests/MicroBoss.Infrastructure.Tests/MicroBoss.Infrastructure.Tests.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add tests/MicroBoss.Infrastructure.Tests/MicroBoss.Infrastructure.Tests.csproj package Moq
```

- [ ] **Step 2: Create IRepository and IUnitOfWork interfaces**

```csharp
// src/MicroBoss.Domain/Interfaces/IRepository.cs
using System.Linq.Expressions;

namespace MicroBoss.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(params object[] keyValues);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> Query();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
```

```csharp
// src/MicroBoss.Domain/Interfaces/IUnitOfWork.cs
namespace MicroBoss.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

- [ ] **Step 3: Create MicroBossDbContext**

```csharp
// src/MicroBoss.Infrastructure/Data/MicroBossDbContext.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Infrastructure.Data;

public class MicroBossDbContext : DbContext
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroBossDbContext).Assembly);
    }
}
```

- [ ] **Step 4: Create key EF Core configurations**

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/ComCodeConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ComCodeConfiguration : IEntityTypeConfiguration<ComCode>
{
    public void Configure(EntityTypeBuilder<ComCode> builder)
    {
        builder.ToTable("ComCode");
        builder.HasKey(e => new { e.GroupId, e.CodeId });
        builder.Property(e => e.GroupId).HasMaxLength(20);
        builder.Property(e => e.CodeId).HasMaxLength(20);
        builder.Property(e => e.CodeName).HasMaxLength(100);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/CustomerConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");
        builder.HasKey(e => e.CustomerId);
        builder.Property(e => e.CustomerId).HasMaxLength(20);
        builder.Property(e => e.CustomerName).HasMaxLength(100);
        builder.Property(e => e.ShortName).HasMaxLength(50);
        builder.Property(e => e.AR).HasColumnType("decimal(18,2)");
        builder.Property(e => e.CL).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ASR).HasColumnType("decimal(18,2)");
        builder.Property(e => e.SettleDiscount).HasColumnType("decimal(18,2)");
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/OrderConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(e => e.OrderNo);
        builder.Property(e => e.OrderNo).HasMaxLength(20);
        builder.Property(e => e.InvoiceDiscount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OrderDiscount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.PrePayAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.TaxRate).HasColumnType("decimal(18,4)");
        builder.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.AccountReceivableAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ActualAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Status).HasConversion<int>();
        builder.Property(e => e.TaxType).HasConversion<int?>();
        builder.Property(e => e.InvoiceType).HasConversion<int?>();

        builder.HasOne(e => e.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(e => e.CustomerId);

        builder.HasMany(e => e.OrderDetails)
            .WithOne(d => d.Order)
            .HasForeignKey(d => d.OrderNo);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/OrderDetailConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetails");
        builder.HasKey(e => e.DetailId);
        builder.Property(e => e.DetailId).HasMaxLength(50);
        builder.Property(e => e.ItemPrice).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ItemAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ItemDiscount).HasColumnType("decimal(18,2)");

        builder.HasOne(e => e.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(e => e.ProductId);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/ProductConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(e => e.ProductId);
        builder.Property(e => e.ProductId).HasMaxLength(50);
        builder.Property(e => e.ProductNo).HasMaxLength(50);
        builder.Property(e => e.ProductClass).HasConversion<int?>();
        builder.Property(e => e.Weight).HasColumnType("decimal(18,4)");

        builder.HasOne(e => e.ProductCost)
            .WithOne(c => c.Product)
            .HasForeignKey<ProductCost>(c => c.ProductId);

        builder.HasMany(e => e.ProductStocks)
            .WithOne(s => s.Product)
            .HasForeignKey(s => s.ProductId);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/ProductCostConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductCostConfiguration : IEntityTypeConfiguration<ProductCost>
{
    public void Configure(EntityTypeBuilder<ProductCost> builder)
    {
        builder.ToTable("ProductCost");
        builder.HasKey(e => e.ProductId);
        builder.Property(e => e.MarketPrice).HasColumnType("decimal(18,2)");
        builder.Property(e => e.UnitPriceA).HasColumnType("decimal(18,2)");
        builder.Property(e => e.InPackagePriceA).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OutPackagePriceA).HasColumnType("decimal(18,2)");
        builder.Property(e => e.UnitPriceB).HasColumnType("decimal(18,2)");
        builder.Property(e => e.InPackagePriceB).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OutPackagePriceB).HasColumnType("decimal(18,2)");
        builder.Property(e => e.UnitPriceC).HasColumnType("decimal(18,2)");
        builder.Property(e => e.InPackagePriceC).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OutPackagePriceC).HasColumnType("decimal(18,2)");
        builder.Property(e => e.LastInStockPrice).HasColumnType("decimal(18,2)");
        builder.Property(e => e.UnitCost).HasColumnType("decimal(18,2)");
        builder.Property(e => e.InPackCost).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OutPackCost).HasColumnType("decimal(18,2)");
        builder.Property(e => e.CostGrossRate).HasColumnType("decimal(18,4)");
        builder.Property(e => e.PackageCost).HasColumnType("decimal(18,2)");
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/ProductStockConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("ProductStock");
        builder.HasKey(e => new { e.StockNo, e.ProductId });
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/SupplierConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Supplier");
        builder.HasKey(e => e.SupplierId);
        builder.Property(e => e.SupplierId).HasMaxLength(20);
        builder.Property(e => e.PrepareAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.TransQuotaAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.SettleDiscount).HasColumnType("decimal(18,2)");

        builder.HasMany(e => e.Banks)
            .WithOne(b => b.Supplier)
            .HasForeignKey(b => b.SupplierId);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/SupplierBankConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class SupplierBankConfiguration : IEntityTypeConfiguration<SupplierBank>
{
    public void Configure(EntityTypeBuilder<SupplierBank> builder)
    {
        builder.ToTable("SupplierBank");
        builder.HasKey(e => new { e.SupplierId, e.BankAccount });
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/PurchaseConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchase");
        builder.HasKey(e => e.PurchaseId);
        builder.Property(e => e.PurchaseId).HasMaxLength(20);
        builder.Property(e => e.TaxRate).HasColumnType("decimal(18,4)");
        builder.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.TaxType).HasConversion<int>();
        builder.Property(e => e.InvoiceType).HasConversion<int>();
        builder.Property(e => e.Status).HasConversion<int?>();

        builder.HasMany(e => e.PurchaseDetails)
            .WithOne(d => d.Purchase)
            .HasForeignKey(d => d.PurchaseId);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/PurchaseDetailConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
    {
        builder.ToTable("PurchaseDetail");
        builder.HasKey(e => new { e.PurchaseId, e.ItemId });
        builder.Property(e => e.ItemPrice).HasColumnType("decimal(18,2)");
        builder.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ItemDiscount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ItemStatus).HasConversion<int?>();
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/AccountsReceivableConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class AccountsReceivableConfiguration : IEntityTypeConfiguration<AccountsReceivable>
{
    public void Configure(EntityTypeBuilder<AccountsReceivable> builder)
    {
        builder.ToTable("AccountsReceivable");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.OrderItemAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OrderTaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.OrderTotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.PriorPeriodBalanceAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.ActualAmount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Status).HasConversion<int>();

        builder.HasMany(e => e.Details)
            .WithOne(d => d.AccountsReceivable)
            .HasForeignKey(d => d.AccountsReceivableId);

        builder.HasMany(e => e.Postings)
            .WithOne(p => p.AccountsReceivable)
            .HasForeignKey(p => p.AccountsReceivableId);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/AccountsReceivableDetailConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class AccountsReceivableDetailConfiguration : IEntityTypeConfiguration<AccountsReceivableDetail>
{
    public void Configure(EntityTypeBuilder<AccountsReceivableDetail> builder)
    {
        builder.ToTable("AccountsReceivableDetail");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Type).HasConversion<int>();
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/AccountsReceivablePostingConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class AccountsReceivablePostingConfiguration : IEntityTypeConfiguration<AccountsReceivablePosting>
{
    public void Configure(EntityTypeBuilder<AccountsReceivablePosting> builder)
    {
        builder.ToTable("AccountReceivablePosting");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Type).HasConversion<int>();
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/RowIndexConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class RowIndexConfiguration : IEntityTypeConfiguration<RowIndex>
{
    public void Configure(EntityTypeBuilder<RowIndex> builder)
    {
        builder.ToTable("RowIndex");
        builder.HasKey(e => e.RowKey);
        builder.Property(e => e.RowKey).HasMaxLength(50);
        builder.Property(e => e.RowVersion).IsRowVersion();
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/MaintenancePartLogConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class MaintenancePartLogConfiguration : IEntityTypeConfiguration<MaintenancePartLog>
{
    public void Configure(EntityTypeBuilder<MaintenancePartLog> builder)
    {
        builder.ToTable("MaintenancePartLog");
        builder.HasKey(e => e.PartLogId);
        builder.Property(e => e.PartLogId).HasMaxLength(50);
        builder.Property(e => e.LogType).HasConversion<int>();
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/SpSourceDataConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class SpSourceDataConfiguration : IEntityTypeConfiguration<SpSourceData>
{
    public void Configure(EntityTypeBuilder<SpSourceData> builder)
    {
        builder.ToTable("SPSourceData");
        builder.HasKey(e => e.PN);
        builder.Property(e => e.PN).HasMaxLength(50);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/ProductPartSuitConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductPartSuitConfiguration : IEntityTypeConfiguration<ProductPartSuit>
{
    public void Configure(EntityTypeBuilder<ProductPartSuit> builder)
    {
        builder.ToTable("ProductPartSuit");
        builder.HasKey(e => e.SuitId);
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Configurations/ProductOrderConfiguration.cs
using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
{
    public void Configure(EntityTypeBuilder<ProductOrder> builder)
    {
        builder.ToTable("ProductOrder");
        builder.HasKey(e => e.ProductId);
        builder.Property(e => e.ProductId).HasMaxLength(50);
    }
}
```

- [ ] **Step 5: Create Repository and UnitOfWork implementations**

```csharp
// src/MicroBoss.Infrastructure/Data/Repositories/Repository.cs
using System.Linq.Expressions;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly MicroBossDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MicroBossDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(params object[] keyValues)
        => await _dbSet.FindAsync(keyValues);

    public async Task<List<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);
}
```

```csharp
// src/MicroBoss.Infrastructure/Data/Repositories/UnitOfWork.cs
using System.Collections.Concurrent;
using MicroBoss.Domain.Interfaces;

namespace MicroBoss.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MicroBossDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(MicroBossDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        return (IRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new Repository<T>(_context));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
```

- [ ] **Step 6: Write failing test for Repository**

```csharp
// tests/MicroBoss.Infrastructure.Tests/RepositoryTests.cs
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
        var customer = new Customer
        {
            CustomerId = "C001",
            CustomerName = "測試客戶"
        };

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
```

- [ ] **Step 7: Run tests to verify they pass**

Run: `cd /mnt/c/development/repos/microboss-new && dotnet test tests/MicroBoss.Infrastructure.Tests/ -v normal`
Expected: 3 tests passed

- [ ] **Step 8: Commit**

```bash
cd /mnt/c/development/repos/microboss-new
git add src/MicroBoss.Domain/Interfaces/ src/MicroBoss.Infrastructure/ tests/MicroBoss.Infrastructure.Tests/
git commit -m "feat(infrastructure): add EF Core DbContext, configurations, repository pattern with tests"
```

---

### Task 5: 橫切關注點 - Serilog + FluentValidation + Mapster + Exception Middleware

**Files:**
- Modify: `src/MicroBoss.Web/MicroBoss.Web.csproj`
- Modify: `src/MicroBoss.Application/MicroBoss.Application.csproj`
- Create: `src/MicroBoss.Web/Middleware/GlobalExceptionMiddleware.cs`
- Create: `src/MicroBoss.Application/Common/PagedResult.cs`
- Create: `src/MicroBoss.Application/Common/ISequenceGenerator.cs`
- Create: `src/MicroBoss.Infrastructure/Data/Services/SequenceGenerator.cs`
- Create: `src/MicroBoss.Application/DependencyInjection.cs`
- Create: `src/MicroBoss.Infrastructure/DependencyInjection.cs`
- Modify: `src/MicroBoss.Web/Program.cs`

- [ ] **Step 1: Add NuGet packages**

Run:
```bash
cd /mnt/c/development/repos/microboss-new
dotnet add src/MicroBoss.Web/MicroBoss.Web.csproj package Serilog.AspNetCore
dotnet add src/MicroBoss.Application/MicroBoss.Application.csproj package FluentValidation.DependencyInjectionExtensions
dotnet add src/MicroBoss.Application/MicroBoss.Application.csproj package Mapster
dotnet add src/MicroBoss.Application/MicroBoss.Application.csproj package Mapster.DependencyInjection
dotnet add src/MicroBoss.Web/MicroBoss.Web.csproj package Microsoft.EntityFrameworkCore.SqlServer
```

- [ ] **Step 2: Create PagedResult and ISequenceGenerator**

```csharp
// src/MicroBoss.Application/Common/PagedResult.cs
namespace MicroBoss.Application.Common;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
```

```csharp
// src/MicroBoss.Application/Common/ISequenceGenerator.cs
namespace MicroBoss.Application.Common;

public interface ISequenceGenerator
{
    Task<string> GetNextOrderNoAsync();
    Task<string> GetNextPurchaseNoAsync();
    Task<string> GetNextSupplierIdAsync();
}
```

- [ ] **Step 3: Create SequenceGenerator implementation**

```csharp
// src/MicroBoss.Infrastructure/Data/Services/SequenceGenerator.cs
using MicroBoss.Application.Common;
using MicroBoss.Domain.Entities;
using MicroBoss.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        if (next > 99999)
            throw new InvalidOperationException("Order sequence overflow");

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
        if (next > 9999)
            throw new InvalidOperationException("Purchase sequence overflow");

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
```

- [ ] **Step 4: Create GlobalExceptionMiddleware**

```csharp
// src/MicroBoss.Web/Middleware/GlobalExceptionMiddleware.cs
using System.Net;
using System.Text.Json;

namespace MicroBoss.Web.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            else
            {
                throw;
            }
        }
    }
}
```

- [ ] **Step 5: Create DI registration extensions**

```csharp
// src/MicroBoss.Application/DependencyInjection.cs
using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
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

        return services;
    }
}
```

```csharp
// src/MicroBoss.Infrastructure/DependencyInjection.cs
using MicroBoss.Application.Common;
using MicroBoss.Domain.Interfaces;
using MicroBoss.Infrastructure.Data;
using MicroBoss.Infrastructure.Data.Repositories;
using MicroBoss.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBoss.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MicroBossDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ISequenceGenerator, SequenceGenerator>();

        return services;
    }
}
```

- [ ] **Step 6: Update Program.cs with all services**

Replace the content of `src/MicroBoss.Web/Program.cs` with:

```csharp
// src/MicroBoss.Web/Program.cs
using MicroBoss.Application;
using MicroBoss.Infrastructure;
using MicroBoss.Web.Components;
using MicroBoss.Web.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/microboss-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddControllers();

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapControllers();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
```

- [ ] **Step 7: Update appsettings.json with connection string**

Replace `src/MicroBoss.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=micro_boss;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

- [ ] **Step 8: Build to verify**

Run: `cd /mnt/c/development/repos/microboss-new && dotnet build MicroBoss.slnx`
Expected: Build succeeded, 0 errors

- [ ] **Step 9: Commit**

```bash
cd /mnt/c/development/repos/microboss-new
git add -A
git commit -m "feat: add cross-cutting concerns - Serilog, FluentValidation, Mapster, exception middleware, DI setup"
```

---

### Task 6: Mapping 設定 + 基礎 DTOs

**Files:**
- Create: `src/MicroBoss.Application/DTOs/ComCodeDto.cs`
- Create: `src/MicroBoss.Application/DTOs/CustomerDto.cs`
- Create: `src/MicroBoss.Application/DTOs/SupplierDto.cs`
- Create: `src/MicroBoss.Application/DTOs/ProductDto.cs`
- Create: `src/MicroBoss.Application/DTOs/OrderDto.cs`
- Create: `src/MicroBoss.Application/DTOs/PurchaseDto.cs`
- Create: `src/MicroBoss.Application/DTOs/InventoryDto.cs`
- Create: `src/MicroBoss.Application/DTOs/MaintenanceLogDto.cs`
- Create: `src/MicroBoss.Application/DTOs/AccountsReceivableDto.cs`
- Create: `src/MicroBoss.Application/DTOs/SpSourceDataDto.cs`
- Create: `src/MicroBoss.Application/Mapping/MappingConfig.cs`

- [ ] **Step 1: Create all DTO files**

```csharp
// src/MicroBoss.Application/DTOs/ComCodeDto.cs
namespace MicroBoss.Application.DTOs;

public record ComCodeDto(string GroupId, string CodeId, string CodeName, int? SortIndex);
public record CreateComCodeDto(string GroupId, string CodeId, string CodeName, int? SortIndex);
public record UpdateComCodeDto(string CodeName, int? SortIndex);
```

```csharp
// src/MicroBoss.Application/DTOs/CustomerDto.cs
namespace MicroBoss.Application.DTOs;

public class CustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? ShortName { get; set; }
    public string? CustomerType { get; set; }
    public string? UnitNo { get; set; }
    public string? TaxType { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? Boss { get; set; }
    public string? ContactWindow { get; set; }
    public string? AccountWindow { get; set; }
    public string? RegisterAddress { get; set; }
    public string? RegisterAddressZip { get; set; }
    public string? InvoiceAddress { get; set; }
    public string? InvoiceAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? CustomerLevel { get; set; }
    public int? SettleDay { get; set; }
    public string? PaymentMethod { get; set; }
    public int? CheckDay { get; set; }
    public string? DeliveryMethod { get; set; }
    public decimal? SettleDiscount { get; set; }
    public string? Note { get; set; }
    public decimal? AR { get; set; }
    public decimal? CL { get; set; }
    public string? SalesId { get; set; }
}

public class CreateCustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? ShortName { get; set; }
    public string? CustomerType { get; set; }
    public string? UnitNo { get; set; }
    public string? TaxType { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? Tel1 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? ContactWindow { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? PaymentMethod { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? Note { get; set; }
}

public class CustomerQueryDto
{
    public string? Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/SupplierDto.cs
namespace MicroBoss.Application.DTOs;

public class SupplierDto
{
    public string SupplierId { get; set; } = string.Empty;
    public string? SupplierShortName { get; set; }
    public string? SupplierFullName { get; set; }
    public string? UniteTitle { get; set; }
    public string? SupplierType { get; set; }
    public string? TaxType { get; set; }
    public string? Tel1 { get; set; }
    public string? Tel2 { get; set; }
    public string? Fax { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? UniteNo { get; set; }
    public string? Boss { get; set; }
    public string? ContactWindow { get; set; }
    public string? AccountWindow { get; set; }
    public string? RegisterAddress { get; set; }
    public string? RegisterAddressZip { get; set; }
    public string? PaymentType { get; set; }
    public int? SettleDay { get; set; }
    public string? Note { get; set; }
    public List<SupplierBankDto> Banks { get; set; } = new();
}

public class SupplierBankDto
{
    public string BankAccount { get; set; } = string.Empty;
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? BankTel { get; set; }
    public string? ItemNote { get; set; }
}

public class CreateSupplierDto
{
    public string? SupplierShortName { get; set; }
    public string? SupplierFullName { get; set; }
    public string? UniteTitle { get; set; }
    public string? SupplierType { get; set; }
    public string? TaxType { get; set; }
    public string? Tel1 { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? ContactWindow { get; set; }
    public string? Note { get; set; }
}

public class SupplierQueryDto
{
    public string? Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/ProductDto.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class ProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameExt { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? Manufacturer { get; set; }
    public bool? IsSuspend { get; set; }
    public string? Voltage { get; set; }
    public ProductCostDto? CostData { get; set; }
    public List<ProductStockDto> StocksData { get; set; } = new();
}

public class ProductCostDto
{
    public string? Currency { get; set; }
    public decimal? MarketPrice { get; set; }
    public decimal? UnitPriceA { get; set; }
    public decimal? UnitPriceB { get; set; }
    public decimal? UnitPriceC { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? LastInStockPrice { get; set; }
    public DateTime? LastInStockDate { get; set; }
    public string? LastInStockSupplier { get; set; }
    public string? PriorityFirstSupplier { get; set; }
}

public class ProductStockDto
{
    public string StockNo { get; set; } = string.Empty;
    public int? StockQty { get; set; }
    public int? SafeQty { get; set; }
    public string? Location { get; set; }
    public int? TransitQty { get; set; }
}

public class CreateProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameExt { get; set; }
    public string? BaseUnit { get; set; }
    public string? BaseBarcode { get; set; }
    public string? Manufacturer { get; set; }
    public string? DefaultStockNo { get; set; }
}

public class ProductQueryDto
{
    public string? Keyword { get; set; }
    public ProductClass? ProductClass { get; set; }
    public string? MainCategory { get; set; }
    public bool? IsSuspend { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/OrderDto.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class OrderDto
{
    public string OrderNo { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public TaxType? TaxType { get; set; }
    public InvoiceType? InvoiceType { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? OrderNote { get; set; }
    public OrderStatus Status { get; set; }
    public decimal OrderDiscount { get; set; }
    public decimal InvoiceDiscount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal PrePayAmount { get; set; }
    public decimal AccountReceivableAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public string? CreateOperator { get; set; }
    public DateTime CreateTime { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; } = new();
}

public class OrderDetailDto
{
    public string DetailId { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductName { get; set; }
    public string? BaseUnit { get; set; }
    public string? OrderUnit { get; set; }
    public int OrderQty { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal ItemAmount { get; set; }
    public decimal ItemDiscount { get; set; }
    public string? ItemNote { get; set; }
}

public class CreateOrderDto
{
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public int? TaxType { get; set; }
    public int? InvoiceType { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ContactWindow { get; set; }
    public string? DeliveryAddressZip { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? UniteNo { get; set; }
    public string? InvoiceTitle { get; set; }
    public string? OrderNote { get; set; }
    public decimal PrePayAmount { get; set; }
    public List<CreateOrderDetailDto> OrderDetails { get; set; } = new();
}

public class CreateOrderDetailDto
{
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductName { get; set; }
    public string? BaseUnit { get; set; }
    public string? OrderUnit { get; set; }
    public int OrderQty { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal ItemDiscount { get; set; }
    public string? ItemNote { get; set; }
}

public class OrderQueryDto
{
    public string? Keyword { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? CustomerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/PurchaseDto.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class PurchaseDto
{
    public string PurchaseId { get; set; } = string.Empty;
    public string? SupplierId { get; set; }
    public string? SupplierShortName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public TaxType TaxType { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public PurchaseStatus? Status { get; set; }
    public string? PurchaseNote { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public DateTime? CreateTime { get; set; }
    public List<PurchaseItemDto> Details { get; set; } = new();
}

public class PurchaseItemDto
{
    public string ItemId { get; set; } = string.Empty;
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductChtName { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Qty { get; set; }
    public decimal? SubTotal { get; set; }
    public PurchaseItemStatus? ItemStatus { get; set; }
    public string? StockNo { get; set; }
    public string? ItemNote { get; set; }
}

public class CreatePurchaseDto
{
    public string? SupplierId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public int TaxType { get; set; }
    public int InvoiceType { get; set; }
    public string? PurchaseNote { get; set; }
    public List<CreatePurchaseItemDto> Details { get; set; } = new();
}

public class CreatePurchaseItemDto
{
    public string? ProductId { get; set; }
    public string? ProductNo { get; set; }
    public string? ProductChtName { get; set; }
    public decimal? ItemPrice { get; set; }
    public int? Qty { get; set; }
    public string? StockNo { get; set; }
    public string? ItemNote { get; set; }
}

public class PurchaseQueryDto
{
    public string? Keyword { get; set; }
    public PurchaseStatus? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/InventoryDto.cs
namespace MicroBoss.Application.DTOs;

public class InventoryDto
{
    public string ProductId { get; set; } = string.Empty;
    public string? ProductNo { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameExt { get; set; }
    public string? Manufacturer { get; set; }
    public string? Voltage { get; set; }
    public string? BaseBarcode { get; set; }
    public string? BaseUnit { get; set; }
    public bool? IsSuspend { get; set; }
    public string? StockNo { get; set; }
    public int SafeQty { get; set; }
    public int CurrentStockQty { get; set; }
    public int TransitQty { get; set; }
}

public class InventoryQueryDto
{
    public string? Keyword { get; set; }
    public string? StockNo { get; set; }
    public bool? UnderSafeQty { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/MaintenanceLogDto.cs
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
```

```csharp
// src/MicroBoss.Application/DTOs/AccountsReceivableDto.cs
using MicroBoss.Domain.Enums;

namespace MicroBoss.Application.DTOs;

public class AccountsReceivableDto
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public DateTime SettleDate { get; set; }
    public string? CustomerId { get; set; }
    public decimal OrderItemAmount { get; set; }
    public decimal OrderTaxAmount { get; set; }
    public decimal OrderTotalAmount { get; set; }
    public decimal PriorPeriodBalanceAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public AccountsReceivableStatus Status { get; set; }
    public string? Note { get; set; }
    public List<AccountsReceivableDetailDto> Details { get; set; } = new();
    public List<AccountsReceivablePostingDto> Postings { get; set; } = new();
}

public class AccountsReceivableDetailDto
{
    public int Id { get; set; }
    public AccountsReceivableType Type { get; set; }
    public string? OrderNo { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

public class AccountsReceivablePostingDto
{
    public int Id { get; set; }
    public DateTime PostingDate { get; set; }
    public AccountsReceivablePostingType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Account { get; set; }
    public string? Bank { get; set; }
    public string? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public string? Note { get; set; }
}

public class CreateAccountsReceivablePostingDto
{
    public DateTime PostingDate { get; set; }
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public string? Account { get; set; }
    public string? Bank { get; set; }
    public string? CheckNumber { get; set; }
    public DateTime? CheckDate { get; set; }
    public string? Note { get; set; }
}

public class AccountsReceivableQueryDto
{
    public string? CustomerId { get; set; }
    public AccountsReceivableStatus? Status { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

```csharp
// src/MicroBoss.Application/DTOs/SpSourceDataDto.cs
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
```

- [ ] **Step 2: Create Mapster mapping configuration**

```csharp
// src/MicroBoss.Application/Mapping/MappingConfig.cs
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
```

- [ ] **Step 3: Build to verify**

Run: `cd /mnt/c/development/repos/microboss-new && dotnet build MicroBoss.slnx`
Expected: Build succeeded, 0 errors

- [ ] **Step 4: Commit**

```bash
cd /mnt/c/development/repos/microboss-new
git add src/MicroBoss.Application/DTOs/ src/MicroBoss.Application/Mapping/
git commit -m "feat(application): add all DTOs and Mapster mapping configuration"
```

---

## Phase 2-6: 後續階段摘要

以下階段將在 Phase 1 完成後，以獨立的計畫文件詳細展開：

### Phase 2: 認證與授權 (Task 7-9)
- Task 7: ASP.NET Core Identity 設定 + 使用者遷移腳本
- Task 8: 登入頁面 + Cookie 認證 + 角色授權
- Task 9: 使用者管理 API + Blazor 頁面

### Phase 3: 核心業務 - 基礎管理 (Task 10-17)
- Task 10: IComCodeService + ComCodeService + API + Blazor 頁面
- Task 11: ICustomerService + CustomerService + API
- Task 12: Customer Blazor 頁面 (列表+編輯)
- Task 13: ISupplierService + SupplierService + API
- Task 14: Supplier Blazor 頁面 (列表+編輯)
- Task 15: IProductService + ProductService + API
- Task 16: Product Blazor 頁面 (列表+編輯)
- Task 17: MainLayout + NavMenu 側邊欄導覽

### Phase 4: 核心業務 - 交易流程 (Task 18-24)
- Task 18: IOrderService + OrderService + 流水號生成
- Task 19: Order API + Blazor 頁面
- Task 20: IPurchaseService + PurchaseService
- Task 21: Purchase API + Blazor 頁面
- Task 22: IInventoryService + InventoryService
- Task 23: Inventory API + Blazor 頁面 (庫存列表 + 安全庫存)
- Task 24: POS 訂單頁面

### Phase 5: 財務模組 (Task 25-28)
- Task 25: IAccountsReceivableService + 應收帳款服務
- Task 26: 應收帳款 API + Blazor 頁面
- Task 27: IInvoiceService + 發票匯出服務
- Task 28: 發票匯出 Blazor 頁面

### Phase 6: 報表列印 + 資料匯入 (Task 29-32)
- Task 29: DevExpress Blazor Reporting 設定 + 出貨單報表
- Task 30: 請款單 + 應收帳款報表 + 客戶信封 + 安全庫存報表 + 發票列印
- Task 31: Bosch 零件匯入 + 產品/庫存匯入
- Task 32: 維修紀錄模組 + 最終收尾

---

## 驗證清單

每個 Task 完成後必須通過：

1. `dotnet build MicroBoss.slnx` — 0 errors
2. `dotnet test` — 所有測試通過
3. 相關功能可在瀏覽器中操作（UI 相關 Task）
