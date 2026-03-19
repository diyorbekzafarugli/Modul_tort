namespace Lesson_4_HttpClient.Api.Models;

public class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
}
