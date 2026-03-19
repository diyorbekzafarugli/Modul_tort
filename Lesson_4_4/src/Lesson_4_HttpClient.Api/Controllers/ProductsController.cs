using Lesson_4_HttpClient.Api.DTOs;
using Lesson_4_HttpClient.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lesson_4_HttpClient.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<List<ProductGetDto>> GetAll()
    {
        var products = _productService.GetAll();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProductGetDto> GetById(Guid id)
    {
        var product = _productService.GetById(id);
        if (product is null) return NotFound($"Product with id {id} not found");
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Guid> Create(ProductCreateDto dto)
    {
        var id = _productService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Update(Guid id, ProductUpdateDto dto)
    {
        dto.Id = id;
        var result = _productService.Update(dto);
        if (!result) return NotFound($"Product with id {id} not found");
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var result = _productService.Delete(id);
        if (!result) return NotFound($"Product with id {id} not found");
        return NoContent();
    }
}