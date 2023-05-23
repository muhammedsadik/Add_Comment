using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
  public interface ICommentRepository : IRepository<Comment>
  {
    List<Comment> GetByProductId(int id);
    Comment GetCommentById(int id);
    bool IsCommented(string userId, int orderItemId);

  }
}
