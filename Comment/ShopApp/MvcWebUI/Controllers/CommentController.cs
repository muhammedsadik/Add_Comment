using Business.Abstract;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MvcWebUI.Identity;
using MvcWebUI.Models;
using Newtonsoft.Json;

namespace MvcWebUI.Controllers
{
  public class CommentController : Controller
  {
    private ICommentService _commentService;
    private ICartService _cartService;
    private IOrderService _orderService;
    private IProductService _productService;
    private UserManager<User> _userManager;



    public CommentController(ICartService cartService, UserManager<User> userManager, IOrderService orderService, ICommentService commentService, IProductService productService)
    {
      _cartService = cartService;
      _userManager = userManager;
      _orderService = orderService;
      _commentService = commentService;
      _productService = productService;
    }

    [HttpPost]
    public IActionResult CommentDelete(CommentModel model)
    {

      var comment = _commentService.GetCommentById((int)model.Id);
      _commentService.Delete(comment);

      var msg = new AlertMessage()
      {
        Message = $"Comment Deleted...",
        AlertType = "danger"
      };
      TempData["message"] = JsonConvert.SerializeObject(msg);

      return Redirect("/comment/list/" + model.ProductId);
    }

    public async Task<IActionResult> CommentList(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var comments = _commentService.GetByProductId((int)id);
      var products = _productService.GetById((int)id);

      var model = new List<CommentModel>();

      foreach (var item in comments)
      {
        var Name =await _userManager.FindByIdAsync(item.UserId);
        var comment = new CommentModel()
        {
          Id = item.Id,
          Text = item.Text,
          OrderItemId = item.OrderItemId,
          ProductId = (int)id,
          UserId = item.UserId,
          UserName = Name.FirstName,
          UserLastName = products.Name
        };
        model.Add(comment);
      }

      return View(model);
    }

    public async Task<IActionResult> CommentCreate(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var userId = _userManager.GetUserId(User);
      var user = await _userManager.FindByIdAsync(userId);
      var entity = _orderService.GetOrderItems((int)id);


      var model = new CommentModel()
      {
        OrderItemId = entity.Id,
        ProductId = entity.ProductId,
        UserName = user.FirstName,
        UserLastName = user.LastName,
        UserId = userId

      };

      return View(model);
    }

    [HttpPost]
    public IActionResult CommentCreate(CommentModel entity)
    {
      var comment = new Comment()
      {
        Text = entity.Text,
        OrderItemId = entity.OrderItemId,
        UserId = entity.UserId
      };

      _commentService.CreateComment(comment);
      return Redirect("~/");
    }


  }
}
