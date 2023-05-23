using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
  public interface IOrderService
  {
    void Create(Order entity);
    List<Order> GetOrders(string userId);
    OrderItem GetOrderItems(int id);

  }
}
