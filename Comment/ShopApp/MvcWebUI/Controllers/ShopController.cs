using Business.Abstract;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcWebUI.Identity;
using MvcWebUI.Models;
using System;

namespace MvcWebUI.Controllers
{
  public class ShopController : Controller
  {
    private IProductService _productService;
    private ICommentService _commentService;
    private UserManager<User> _userManager;

    public ShopController(IProductService productService, ICommentService commentService, UserManager<User> userManager)
    {
      _productService = productService;
      _commentService = commentService;
      _userManager = userManager;
    }

    public IActionResult List(string category, int page = 1)
    {
      const int pageSize = 2;

      var productList = _productService.GetProductsByCategory(category, page, pageSize);

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
        PageInfo = new PageInfo()
        {
          TotalItems = _productService.GetCountByCategory(category),
          CurrentPage = page,
          ItemsPerPage = pageSize,
          CurrentCategory = category
        },
        Products = products
      };

      return View(productViewModel);
    }


    public async Task<IActionResult> Details(string url)
    {
      if (url == null)
      {
        return NotFound();
      }

      Product product = _productService.GetProductDetails(url);
      if (product == null)
      {
        return NotFound();
      }

      var comment = _commentService.GetByProductId(product.ProductId);
      if (comment == null)
      {
        return NotFound();
      }

      var commentModel = new List<CommentModel>();

      foreach(var item in comment)
      {
        var user = await _userManager.FindByIdAsync(item.UserId);

        commentModel.Add(
          new CommentModel() { Text = item.Text, UserName = user.FirstName }          
          );
      }


      return View(new ProductDetailModel
      {
        Product = product,
        Categories = product.ProductCategories.Select(c => c.Category).ToList(),
        Comments = commentModel
      });
    }

    public IActionResult Search(string q)
    {
      var productList = _productService.GetSearchResult(q);

      var products = new List<ProductListModel>();

      foreach (var item in productList)
      {
        string count = _commentService.GetByProductId(item.ProductId).Count().ToString();
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
          CommentCount = count,
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
  }
}
