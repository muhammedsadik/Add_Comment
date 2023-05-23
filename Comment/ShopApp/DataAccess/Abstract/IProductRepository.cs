using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
  public interface IProductRepository : IRepository<Product>
  {
    Product GetProductDetails(string url);
    Product GetByIdWithCategories(int id);
    void Update(Product entity, int[] categoryIds);
    List<Product> GetProductsByCategory(string name, int page , int pageSize);
    List<Product> GetHomePageProducts();
    List<Product> GetSearchResult(string searchString);
    int GetCountByCategory(string category);

  }
}
