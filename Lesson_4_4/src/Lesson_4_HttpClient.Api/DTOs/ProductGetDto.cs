namespace Lesson_4_HttpClient.Api.DTOs;

public class ProductGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
