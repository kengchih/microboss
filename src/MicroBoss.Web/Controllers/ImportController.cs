using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ImportController : ControllerBase
{
    private readonly IBoschImportService _boschImportService;

    public ImportController(IBoschImportService boschImportService)
    {
        _boschImportService = boschImportService;
    }

    // POST /api/import/bosch
    [HttpPost("bosch")]
    public async Task<IActionResult> ImportBosch([FromBody] List<SpSourceDataDto> items)
    {
        await _boschImportService.ImportAsync(items);
        return Ok(new { message = $"成功匯入 {items.Count} 筆資料" });
    }

    // DELETE /api/import/bosch
    [HttpDelete("bosch")]
    public async Task<IActionResult> ResetBosch()
    {
        await _boschImportService.ResetAsync();
        return Ok(new { message = "Bosch 零件資料已清除" });
    }
}
