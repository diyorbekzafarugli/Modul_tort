using Lesson_4_1_Web.Api.Entities;
using Lesson_4_1_Web.Api.Exceptions;
using Lesson_4_1_Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lesson_4_1_Web.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Add(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name) || price < 0)
            return BadRequest("name or price no correct");

        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };
        return Ok(_service.Add(product));
    }

    [HttpGet("all-products")]
    public IActionResult GetAll()
    {
        try
        {
            var products = _service.GetAll();
            return Ok(products);
        }
        catch (ProductNotFoundException)
        {
            return NotFound("product topilmadi");
        }
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            return Ok(_service.GetById(id));
        }
        catch (ProductNotFoundException)
        {
            return NotFound("Product topilmadi");
        }
    }

    [HttpPut]
    public IActionResult Update(Guid id, string updatedName, decimal updatedPrice)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(updatedName) || updatedPrice < 0)
                return BadRequest("name or price no correct");

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = updatedName,
                Price = updatedPrice
            };
            bool isSuccess = _service.Update(product);
            if (!isSuccess)
                return BadRequest("xatolik");
            return Ok("yangilandi");
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        bool isSuccess = _service.Delete(id);
        if (!isSuccess)
            return BadRequest("o'chirishda xatolik");
        return Ok("o'chirildi");
    }
}
