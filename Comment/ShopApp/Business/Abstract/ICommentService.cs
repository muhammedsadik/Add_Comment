using Entity;



namespace Business.Abstract
{
  public interface ICommentService
  {
    List<Comment> GetAll();
    Comment GetCommentById(int id);
    List<Comment> GetByProductId(int id);
    void Delete(Comment entity);
    void CreateComment(Comment entity);
    bool IsCommented(string userId, int orderItemId);

  }
}
