using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using MvcWebUI.Models;

namespace MvcWebUI.Controllers
{
  public class HomeController : Controller
  {
    private IProductService _productService;
    private ICommentService _commentService;

    public HomeController(IProductService productService, ICommentService commentService)
    {
      this._productService = productService;
      this._commentService = commentService;
    }

    public IActionResult Index()
    {

      //var productViewModel = new ProductListViewModel()
      //{
      //  Products = _productService.GetHomePageProducts()
      //};

      //return View(productViewModel);



      var productList = _productService.GetHomePageProducts();

      var products = new List<ProductListModel>();

      foreach (var item in productList)
      {
        string num = _commentService.GetByProductId(item.ProductId).Count().ToString();
        var model = new ProductListModel()
        {
          ProductId = item.ProductId,
          Name = item.Name,
          Url = item.Url,
          Price = item.Price,
          Description = item.Description,
          ImageUrl = item.ImageUrl,
          IsApproved = item.IsApproved,
          IsHome = item.IsHome,
          DateAdded = item.DateAdded,
          CommentCount = num,
          ProductCategories = item.ProductCategories

        };
        products.Add(model);
      }

      var productViewModel = new ProductListViewModel()
      {
        Products = products
      };

      return View(productViewModel);
    }










    public IActionResult About()
    {

      return View();
    }

  }
}