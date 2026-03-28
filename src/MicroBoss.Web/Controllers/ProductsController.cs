using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> Query(
        [FromQuery] string? keyword,
        [FromQuery] ProductClass? productClass,
        [FromQuery] string? mainCategory,
        [FromQuery] bool? isSuspend,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new ProductQueryDto
        {
            Keyword = keyword,
            ProductClass = productClass,
            MainCategory = mainCategory,
            IsSuspend = isSuspend,
            Page = page,
            PageSize = pageSize
        };
        return Ok(await _productService.QueryAsync(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpGet("by-no/{productNo}")]
    public async Task<ActionResult<ProductDto>> GetByNo(string productNo)
    {
        var product = await _productService.GetByNoAsync(productNo);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = dto.ProductId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CreateProductDto dto)
    {
        await _productService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
