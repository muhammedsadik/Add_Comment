using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
  public class OrderManager : IOrderService
  {
    private readonly IUnitOfWork _unitOfWork;

    public OrderManager(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public void Create(Order entity)
    {
      _unitOfWork.Orders.Create(entity);
      _unitOfWork.Save();
    }

    public List<Order> GetOrders(string userId)
    {
      return _unitOfWork.Orders.GetOrders(userId);
    }

    public OrderItem GetOrderItems(int id)
    {
      return _unitOfWork.Orders.GetOrdersItem(id);
    }
  }
}
