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
