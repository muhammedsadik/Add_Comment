



using Business.Abstract;
using DataAccess.Abstract;
using Entity;

namespace Business.Concrete
{
  public class CommentManager : ICommentService
  {
    private readonly IUnitOfWork _unitOfWork;
    public CommentManager(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public bool IsCommented(string userId, int orderItemId)
    {
      return _unitOfWork.Comments.IsCommented(userId, orderItemId);
    }

    public void CreateComment(Comment entity)
    {
      _unitOfWork.Comments.Create(entity);
      _unitOfWork.Save();
    }

    public void Delete(Comment entity)
    {
      _unitOfWork.Comments.Delete(entity);
      _unitOfWork.Save();
    }

    public List<Comment> GetAll()
    {
      return _unitOfWork.Comments.GetAll();
    }

    public List<Comment> GetByProductId(int id)
    {
      return _unitOfWork.Comments.GetByProductId(id);
    }

    public Comment GetCommentById(int id)
    {
      return _unitOfWork.Comments.GetCommentById(id);
    }

  }
}
