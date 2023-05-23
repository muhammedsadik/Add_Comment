using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
  public interface ICartRepository : IRepository<Cart>
  {
    Cart GetByUserId(string userId);
    void DeleteFromCart(int cartId, int productId);
    void ClearCart(int cartId);
  }
}
