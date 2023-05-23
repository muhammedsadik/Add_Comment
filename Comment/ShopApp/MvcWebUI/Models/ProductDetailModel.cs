using Entity;

namespace MvcWebUI.Models
{
  public class ProductDetailModel
  {
    public Product Product { get; set; }
    public List<Category> Categories { get; set; }
    public List<CommentModel> Comments { get; set; }
  }
}
