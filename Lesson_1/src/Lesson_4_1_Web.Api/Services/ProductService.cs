using Lesson_4_1_Web.Api.Entities;
using Lesson_4_1_Web.Api.Exceptions;
using System.Text.Json;

namespace Lesson_4_1_Web.Api.Services;

public class ProductService : IProductService
{
    private readonly string _filePath;
    private readonly static JsonSerializerOptions _options = new() { WriteIndented = true };

    public ProductService()
    {
        var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        Directory.CreateDirectory(directoryPath);

        _filePath = Path.Combine(directoryPath, $"Products.json");
        if (!File.Exists(_filePath)) File.WriteAllText(_filePath, "[]");
    }

    private void WriteAllToFile(List<Product> products)
    {
        var json = JsonSerializer.Serialize(products, _options);
        File.WriteAllText(_filePath, json);
    }

    private List<Product> ReadFromFile()
    {
        var json = File.ReadAllText(_filePath);

        return JsonSerializer.Deserialize<List<Product>>(json, _options) ?? new List<Product>();
    }

    public Guid Add(Product product)
    {
        var products = ReadFromFile();
        products.Add(product);
        WriteAllToFile(products);
        return product.Id;
    }

    public List<Product> GetAll()
    {
        var products = ReadFromFile();
        if (products.Count == 0)
            throw new ProductNotFoundException();
        return products;
    }

    public Product GetById(Guid id)
    {
        var products = ReadFromFile();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product is null)
            throw new ProductNotFoundException();
        return product;
    }

    public bool Update(Product product)
    {
        var products = ReadFromFile();
        var index = products.FindIndex(i => i.Id == product.Id);
        if (index == -1)
            return false;
        products[index] = product;
        WriteAllToFile(products);
        return true;
    }

    public bool Delete(Guid id)
    {
        var products = ReadFromFile();
        int count = products.RemoveAll(p => p.Id == id);
        if (count != 0)
        {
            WriteAllToFile(products);
            return true;
        }

        return false;
    }
}
