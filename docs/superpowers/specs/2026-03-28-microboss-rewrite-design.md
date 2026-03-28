# MicroBoss 系統重寫設計文件

## 概述

將 MicroBoss ERP 系統從 .NET Framework 4.5 (WinForms + WCF + EF6) 全面重寫為 .NET 10 (Blazor Server + REST API + EF Core)，採用 Clean Architecture 分層架構。

**目標：**
- 技術升級至 .NET 10
- 改善程式碼結構與可維護性
- 提升可測試性
- Web 化取代桌面應用程式
- 保留完整的套表列印與發票列印能力

**遷移策略：** Big Bang — 全部重寫完成後一次性切換上線。

**部署方式：** On-premise (自建伺服器)，單一公司使用。

---

## 1. Solution 結構與分層

```
MicroBoss.sln
│
├─ src/
│   ├─ MicroBoss.Domain/              # 領域層（零依賴）
│   │   ├─ Entities/                   # EF Core 實體
│   │   ├─ Enums/                      # 列舉定義
│   │   └─ Interfaces/                 # IRepository<T>, IUnitOfWork
│   │
│   ├─ MicroBoss.Application/         # 應用層（業務邏輯）
│   │   ├─ Services/                   # 業務服務實作
│   │   ├─ DTOs/                       # 資料傳輸物件
│   │   ├─ Interfaces/                 # 服務介面
│   │   └─ Mapping/                    # Entity <-> DTO 映射
│   │
│   ├─ MicroBoss.Infrastructure/      # 基礎設施層
│   │   ├─ Data/
│   │   │   ├─ MicroBossDbContext.cs
│   │   │   ├─ Configurations/         # Fluent API 實體設定
│   │   │   ├─ Migrations/
│   │   │   └─ Repositories/
│   │   ├─ Identity/                   # ASP.NET Core Identity 設定
│   │   └─ DependencyInjection.cs
│   │
│   └─ MicroBoss.Web/                 # Blazor Server 主專案
│       ├─ Controllers/                # REST API
│       ├─ Components/
│       │   ├─ Pages/                  # Blazor 頁面
│       │   ├─ Shared/                 # 共用元件
│       │   └─ Controls/              # 可復用控制項
│       ├─ Reports/                    # DevExpress 報表
│       ├─ wwwroot/
│       └─ Program.cs
│
├─ tests/
│   ├─ MicroBoss.Application.Tests/
│   └─ MicroBoss.Infrastructure.Tests/
│
└─ docs/
```

**依賴方向：** `Web → Application → Domain ← Infrastructure`

Domain 層不依賴任何其他層。Infrastructure 實作 Domain 定義的介面。

---

## 2. 領域實體與資料庫遷移

### 2.1 遷移流程

1. 使用 `dotnet ef dbcontext scaffold` 從現有 `micro_boss` 資料庫反向生成 Code-first 實體
2. 手動整理命名、導覽屬性、列舉轉換
3. 建立 Initial Migration（對齊現有 DB schema，不實際改表）
4. 後續變更透過 EF Core Migrations 管理

### 2.2 實體對應

| 現有 DAL 實體 | 新 Domain Entity | 說明 |
|---|---|---|
| Orders + OrderDetails | Order, OrderDetail | 訂單 |
| Product, ProductCost, ProductStock, ProductPartSuit | Product, ProductCost, ProductStock, ProductPartSuit | 產品相關 |
| Purchase + PurchaseDetail | Purchase, PurchaseDetail | 採購 |
| Customer | Customer | 客戶 |
| Supplier + SupplierBank | Supplier, SupplierBank | 供應商 |
| AccountsReceivable, AccountsReceivableDetail, AccountReceivablePosting | AccountsReceivable, AccountsReceivableDetail, AccountsReceivablePosting | 應收帳款 |
| User, UserRole, RolePermission | ASP.NET Core Identity | 使用者與權限 |
| ComCode | ComCode | 輔助代碼 |
| MaintenancePartLog | MaintenancePartLog | 維修紀錄 |
| RowIndex | RowIndex | 流水號管理 |
| SPSourceData | SpSourceData | Bosch 零件資料 |
| ProductOrder | ProductOrder | 產品訂單歷史 |

### 2.3 列舉統一

現有 Dictionary 類別改為 C# enum：

```csharp
public enum OrderStatus { Draft, Confirmed, Shipped, Completed, Cancelled }
public enum TaxType { Taxable, TaxFree, ZeroTax }
public enum InvoiceType { Duplicate, Triplicate }
public enum PurchaseStatus { Draft, Confirmed, Received, Completed, Cancelled }
public enum PurchaseItemStatus { Pending, PartialReceived, Received }
public enum StockActionType { In, Out, Adjust }
public enum ProductClass { /* 依現有資料定義 */ }
public enum DataStatus { Active, Inactive }
public enum ComGroup { /* 依現有資料定義 */ }
```

### 2.4 認證遷移

- 現有 `User` + `UserRole` + `RolePermission` + WCF `CustomUserNameValidator` 改為 ASP.NET Core Identity
- 寫遷移腳本將既有使用者資料匯入 Identity 表
- 角色：`Admin`, `Sales`, `Purchase`, `Warehouse`, `Accounting`
- Cookie-based 認證（適合 Blazor Server）

---

## 3. 應用層服務設計

### 3.1 服務對應

| 現有 WCF 服務 | 新 Application Service | 職責 |
|---|---|---|
| IOrderService | IOrderService | 訂單 CRUD、建單流程、出貨 |
| IPurchaseService | IPurchaseService | 採購單 CRUD、進貨流程 |
| IManagementService | IProductService | 產品 CRUD、庫存管理、安全庫存 |
| IAccountService | IAccountService | 使用者帳號管理（搭配 Identity） |
| IAssestService | IInventoryService | 盤點、庫存異動 |
| ICommonService | IComCodeService | 輔助代碼管理 |
| IBoschService | IBoschImportService | Bosch 零件資料匯入 |
| （新增） | IAccountsReceivableService | 應收帳款、銷帳、請款 |
| （新增） | IReportService | 報表資料查詢 |
| （新增） | IInvoiceService | 發票匯出、列印次數統計 |

### 3.2 服務模式

```csharp
public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(int id);
    Task<PagedResult<OrderDto>> QueryAsync(OrderQueryDto query);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task UpdateAsync(int id, UpdateOrderDto dto);
    Task DeleteAsync(int id);
    Task ConfirmAsync(int id);
    Task ShipAsync(int id, ShipOrderDto dto);
}

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISequenceGenerator _sequenceGenerator;
    // 透過 DI 注入，不再 new DataEntities()
}
```

### 3.3 DTO 映射

使用 Mapster 或 AutoMapper 取代手動屬性對拷。每個服務對應的 DTO 組：
- `XxxDto` — 查詢回傳
- `CreateXxxDto` — 新增用
- `UpdateXxxDto` — 更新用
- `XxxQueryDto` — 查詢條件

### 3.4 流水號生成

- 保留 `RowIndex` 表
- 用 EF Core 樂觀鎖（ConcurrencyToken / RowVersion）處理並發
- 封裝為 `ISequenceGenerator` 服務

### 3.5 橫切關注點

- **驗證：** FluentValidation
- **日誌：** Serilog（寫入檔案）
- **例外處理：** 全域 Exception Middleware 統一回傳格式
- **映射：** Mapster 或 AutoMapper

---

## 4. Web 層與 UI

### 4.1 技術選擇

- Blazor Server + DevExpress Blazor Components
- DxGrid（資料表格）、DxFormLayout（表單）、DxPopup（對話框）、DxMenu/DxTreeView（導覽）
- DevExpress Blazor Report Viewer（報表預覽與列印）

### 4.2 頁面對應

| 現有 WinForms Module/Form | 新 Blazor Page | 路由 |
|---|---|---|
| OrderManagementModule + OrderEditForm | Orders.razor, OrderEdit.razor | /orders, /orders/{id} |
| PurchaseModule + PurchaseEditForm | Purchases.razor, PurchaseEdit.razor | /purchases, /purchases/{id} |
| ProductManagementModule + ProductEditForm | Products.razor, ProductEdit.razor | /products, /products/{id} |
| CustomerManagementModule + CustomerEditForm | Customers.razor, CustomerEdit.razor | /customers, /customers/{id} |
| SupplierManagementModule + SupplierEditForm | Suppliers.razor, SupplierEdit.razor | /suppliers, /suppliers/{id} |
| AR_ManagementModule + AccountsReceivableCreateForm | AccountsReceivable.razor, AREdit.razor | /ar, /ar/{id} |
| AR_PostingModule + AccountsReceivablePostingForm | ARPosting.razor | /ar/posting |
| AccountManagementModule + AccountEditForm | Users.razor, UserEdit.razor | /admin/users, /admin/users/{id} |
| ComCodeManagement + AssistComCodeEditForm | ComCodes.razor, ComCodeEdit.razor | /admin/comcodes |
| MaintenanceLogModule + MaintainLogForm | MaintenanceLogs.razor | /maintenance |
| OrderPosForm / NewOrderPosForm | PosOrder.razor | /pos |
| InvoiceExportForm | InvoiceExport.razor | /invoices/export |
| InventoryStockListForm | Inventory.razor | /inventory |
| UnderSafeQtyProductsForm | SafeStock.razor | /inventory/safe-stock |
| BoschSparePartOrderImportModule | BoschImport.razor | /import/bosch |
| SisProductImportModule / ProductStockImportModule | DataImport.razor | /import/products |
| ProductSearchForm | 整合到 Products.razor 搜尋功能 | — |

### 4.3 報表列印

流程：使用者點選列印 → Blazor 呼叫 ReportService → DevExpress Report Viewer 載入報表模板 → 瀏覽器內預覽 → 列印 / 匯出 PDF

報表需用 DevExpress Blazor Report Designer 重新設計模板，參考原始報表定義：

| 現有報表 | 用途 |
|---|---|
| ShipmentOrderReport | 出貨單 |
| RequestBillingReport | 請款單 |
| AccountsReceivableReport | 應收帳款報表 |
| AddressReport | 客戶信封 |
| UnSafeQtyProductReport | 安全庫存報表 |
| InvoicePrintReport | 發票列印 |

### 4.4 Layout 結構

```
┌─────────────────────────────────────────┐
│  頂部導覽列（登入使用者、登出）           │
├────────┬────────────────────────────────┤
│ 側邊欄  │  主內容區                      │
│        │                                │
│ 訂單管理│  DxGrid / 表單 / 報表預覽      │
│ 採購管理│                                │
│ 產品管理│                                │
│ 客戶管理│                                │
│ 供應商  │                                │
│ 應收帳款│                                │
│ 庫存管理│                                │
│ POS    │                                │
│ 資料匯入│                                │
│ 系統設定│                                │
└────────┴────────────────────────────────┘
```

### 4.5 REST API

API 供 Blazor 頁面呼叫，同時預留給未來行動端使用：

```
/api/orders
/api/purchases
/api/products
/api/customers
/api/suppliers
/api/accounts-receivable
/api/inventory
/api/comcodes
/api/reports
/api/invoices
/api/import
/api/admin/users
```

---

## 5. 基礎設施與部署

### 5.1 EF Core 設定

- 連線字串從 `appsettings.json` 讀取，支援開發/正式環境切換
- 泛型 `Repository<T>` + `UnitOfWork` 封裝 DbContext
- 關鍵實體（Order, Purchase, ProductStock）加入 `RowVersion` 樂觀鎖
- 每個實體一個 Fluent API Configuration 類別

### 5.2 DI 註冊

```csharp
// Program.cs
builder.Services
    .AddInfrastructure(builder.Configuration)  // DbContext, Repositories, Identity
    .AddApplication();                          // Services, Mapping, Validation
```

### 5.3 認證與授權

- ASP.NET Core Identity + Cookie Authentication
- `[Authorize(Roles = "...")]` 控制 API 與頁面存取

### 5.4 日誌

- Serilog 寫入檔案
- 結構化日誌格式

### 5.5 部署架構

```
IIS / Kestrel on Windows Server
  └─ MicroBoss.Web (Blazor Server + API)
       └─ SQL Server (micro_boss)
```

- 不再需要 Windows Service host WCF
- 不再需要 .NET Framework — 只需 .NET 10 Runtime
- 發布為 self-contained 執行檔或 IIS 站台

### 5.6 測試策略

- **MicroBoss.Application.Tests：** xUnit + Moq 測試業務邏輯
- **MicroBoss.Infrastructure.Tests：** xUnit + SQLite In-Memory 測試 Repository 和 DbContext
- 優先為核心流程寫測試：建單、出貨、庫存異動、應收帳款銷帳

---

## 技術棧總覽

| 類別 | 現有 | 新 |
|---|---|---|
| Runtime | .NET Framework 4.5 | .NET 10 |
| UI | DevExpress WinForms | DevExpress Blazor Server |
| 通訊 | WCF netTcpBinding | ASP.NET Core REST API |
| ORM | EF6 Database-first + Dapper + XPO | EF Core Code-first |
| 認證 | 自製 UserNamePasswordValidator | ASP.NET Core Identity |
| 日誌 | log4net | Serilog |
| 驗證 | 手動檢查 | FluentValidation |
| 映射 | 手動屬性對拷 | Mapster / AutoMapper |
| 測試 | 無 | xUnit + Moq |
| 部署 | Windows Service + WinForms ClickOnce | IIS / Kestrel |
