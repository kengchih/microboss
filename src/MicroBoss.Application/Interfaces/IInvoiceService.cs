using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IInvoiceService
{
    Task<List<InvoiceExportDto>> GetInvoiceExportDataAsync(DateTime dateFrom, DateTime dateTo);
}
