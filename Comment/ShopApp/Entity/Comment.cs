using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public class Comment
  {
    public int Id { get; set; }
    public string Text { get; set; }
    public int OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; }
    public string UserId { get; set; }
  }
}
