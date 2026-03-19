using Lesson_4_HttpClient.Api.DTOs;
using Lesson_4_HttpClient.Api.Models;
using System.Text.Json;

namespace Lesson_4_HttpClient.Api.Services;

public class ProductService : IProductService
{
    private readonly string _filePath;
    private static JsonSerializerOptions options = new() { WriteIndented = true };

    public ProductService()
    {
        var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        Directory.CreateDirectory(directory);
        _filePath = Path.Combine(directory, "Products.json");
        if (!File.Exists(_filePath)) return;
    }

    private async Task<List<Product>> ReadAll()
    {
        if (!File.Exists(_filePath)) return [];
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Product>>(json) ?? [];
    }

    private async Task SaveAll(List<Product> products)
    {
        var json = JsonSerializer.Serialize(products, options);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<Guid> Create(ProductCreateDto dto)
    {
        var products = await ReadAll();

        var product = new Product
        {
            Name = dto.Name,
            Category = dto.Category,
            Price = dto.Price
        };

        products.Add(product);
        await SaveAll(products);
        return product.Id;
    }

    public async Task<List<ProductGetDto>> GetAll()
    {
        return await ReadAll()
            .Select(p => new ProductGetDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToListAsync();
    }

    public async Task<ProductGetDto?> GetById(Guid id)
    {
        var products = await ReadAll()
            var por.FirstOrDefault(p => p.Id == id);
        if (product is null) return null;

        return new ProductGetDto
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<bool> Update(ProductUpdateDto dto)
    {
        var products = await ReadAll();
        var product = products.FirstOrDefault(p => p.Id == dto.Id);
        if (product is null) return false;

        if (dto.Name is not null) product.Name = dto.Name;
        if (dto.Category is not null) product.Category = dto.Category;
        if (dto.Price is not null) product.Price = dto.Price.Value;

        product.UpdatedAt = DateTime.UtcNow;

        await SaveAll(products);
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var products = await ReadAll();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product is null) return false;

        products.Remove(product);
        await SaveAll(products);
        return true;
    }
}
