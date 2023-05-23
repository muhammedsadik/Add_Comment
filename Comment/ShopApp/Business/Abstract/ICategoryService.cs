using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
  public interface ICategoryService : IValidator<Category>
  {
    Category GetById(int id);
    List<Category> GetAll();
    Category GetByIdWithProducts(int CategoryId);
    bool Create(Category entity);
    bool Update(Category entity);
    void Delete(Category entity);
    void DeleteFromCategory(int productId,int categoryId);
  }
}
