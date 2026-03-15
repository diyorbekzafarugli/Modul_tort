using Lesson_4_1_Web.Api.Entities;

namespace Lesson_4_1_Web.Api.Services;

public interface IProductService
{
    Guid Add(Product product);
    List<Product> GetAll();
    Product GetById(Guid id);
    bool Update(Product product);
    bool Delete(Guid id);
}
