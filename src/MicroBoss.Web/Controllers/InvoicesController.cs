using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("export")]
    public async Task<ActionResult<List<InvoiceExportDto>>> Export(
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo)
    {
        var data = await _invoiceService.GetInvoiceExportDataAsync(dateFrom, dateTo);
        return Ok(data);
    }
}
