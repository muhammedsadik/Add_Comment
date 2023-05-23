using Entity;
using System.ComponentModel.DataAnnotations;

namespace MvcWebUI.Models
{
  public class CommentModel
  {
    public int Id { get; set; }

    [Display(Name = "comment", Prompt = "Your comment is a guide for us.")]
    [Required]
    public string Text { get; set; }
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserLastName { get; set; }
    public OrderItem OrderItem { get; set; }

  }
}
