namespace Lesson_4_HttpClient.Api.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
