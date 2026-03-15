namespace Lesson_4_1_Web.Api.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException()
    {
    }

    public ProductNotFoundException(string? message) : base(message)
    {
    }

    public ProductNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}