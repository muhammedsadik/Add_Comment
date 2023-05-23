using DataAccess.Abstract;
using Entity;
using Microsoft.EntityFrameworkCore;



namespace DataAccess.Concrete.EfCore
{
  public class EfCoreCommentRepository : EfCoreGenericRipository<Comment>, ICommentRepository
  {
    public EfCoreCommentRepository(DbContext ctx) : base(ctx)
    {
    }

    private ShopContext ShopContext
    {
      get { return context as ShopContext; }
    }


    public bool IsCommented(string userId, int orderItemId)
    {
      var sate = ShopContext.Comments
        .Where(i => i.UserId == userId)
        .Any(x => x.OrderItemId == orderItemId);



      return sate;
    }




    public List<Comment> GetByProductId(int id)
    {
      var comment = ShopContext.Comments
        .Include(i => i.OrderItem)
        .Where(i => i.OrderItem.ProductId == id);

      return comment.ToList();
    }

    public Comment GetCommentById(int id)
    {
      return ShopContext.Comments
        .Where(i=>i.Id==id)
        .FirstOrDefault();
    }

  }
}
