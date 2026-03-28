using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _uow;

    public InvoiceService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<InvoiceExportDto>> GetInvoiceExportDataAsync(DateTime dateFrom, DateTime dateTo)
    {
        var toInclusive = dateTo.Date.AddDays(1);

        var orders = await _uow.Repository<Order>().Query()
            .Include(o => o.OrderDetails)
            .Where(o => o.InvoiceType != null
                     && o.InvoiceType != InvoiceType.None
                     && o.OrderDate >= dateFrom.Date
                     && o.OrderDate < toInclusive)
            .OrderBy(o => o.OrderDate)
            .ThenBy(o => o.OrderNo)
            .ToListAsync();

        return orders.Select(o =>
        {
            var itemAmount = o.OrderDetails.Sum(d => d.ItemAmount);
            return new InvoiceExportDto
            {
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
                CustomerName = o.CustomerName,
                UniteNo = o.UniteNo,
                InvoiceTitle = o.InvoiceTitle,
                InvoiceType = (int)(o.InvoiceType ?? InvoiceType.None),
                ItemAmount = itemAmount,
                TaxAmount = o.TaxAmount,
                TotalAmount = itemAmount + o.TaxAmount
            };
        }).ToList();
    }
}
