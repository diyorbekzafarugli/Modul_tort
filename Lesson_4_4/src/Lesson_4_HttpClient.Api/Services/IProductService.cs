using Lesson_4_HttpClient.Api.DTOs;

namespace Lesson_4_HttpClient.Api.Services;

public interface IProductService
{
    Task<Guid> Create(ProductCreateDto dto);
    Task<List<ProductGetDto>> GetAll();
    Task<ProductGetDto?> GetById(Guid id);
    Task<bool> Update(ProductUpdateDto dto);
    Task<bool> Delete(Guid id);
}
