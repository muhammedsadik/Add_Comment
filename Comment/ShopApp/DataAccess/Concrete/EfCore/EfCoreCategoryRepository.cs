using DataAccess.Abstract;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EfCore
{
  public class EfCoreCategoryRepository : EfCoreGenericRipository<Category>, ICategoryRepository
  {
    public EfCoreCategoryRepository(ShopContext context) : base(context)
    {

    }

    private ShopContext ShopContext
    {
      get { return context as ShopContext; }
    }
    public void DeleteFromCategory(int productId, int categoryId)
    {
        var cmd = "delete from productcategory where ProductId=@p0 and CategoryId=@p1";
      ShopContext.Database.ExecuteSqlRaw(cmd,productId,categoryId);      
    }

    public Category GetByIdWithProducts(int CategoryId)
    {      
        return ShopContext.Categories
          .Where(i => i.CategoryId == CategoryId)
          .Include(i => i.ProductCategories)
          .ThenInclude(i => i.Product)
          .FirstOrDefault();      
    }


  }
}
