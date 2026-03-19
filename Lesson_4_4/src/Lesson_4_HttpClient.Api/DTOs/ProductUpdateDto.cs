namespace Lesson_4_HttpClient.Api.DTOs;

public class ProductUpdateDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public decimal? Price { get; set; }
   
}
