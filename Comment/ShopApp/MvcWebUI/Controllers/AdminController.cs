using Business.Abstract;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.Extensions;
using MvcWebUI.Identity;
using MvcWebUI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Principal;

namespace MvcWebUI.Controllers
{
  [Authorize(Roles = "Admin")]
  public class AdminController : Controller
  {
    private IProductService _productService;
    private ICategoryService _categoryService;
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<User> _userManager;
    private ICommentService _commentService;
    public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ICommentService commentService)
    {
      _productService = productService;
      _categoryService = categoryService;
      _roleManager = roleManager;
      _userManager = userManager;
      _commentService = commentService;
    }

    public async Task<IActionResult> UserEdit(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user != null)
      {
        var SelectedRoles = await _userManager.GetRolesAsync(user);
        var roles = _roleManager.Roles.Select(i => i.Name);

        ViewBag.Roles = roles;
        ViewBag.selected = SelectedRoles;
        return View(new UserDetailsModel()
        {
          UserId = user.Id,
          UserName = user.UserName,
          FirstName = user.FirstName,
          LastName = user.LastName,
          Email = user.Email,
          EmailConfirmed = user.EmailConfirmed,
          SelectedRoles = SelectedRoles

        });
      }
      return Redirect("~/admin/user/list");
    }

    [HttpPost]
    public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
    {

      var user = await _userManager.FindByIdAsync(model.UserId);
      if (user != null)
      {
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.EmailConfirmed = model.EmailConfirmed;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
          var userRoles = await _userManager.GetRolesAsync(user);
          selectedRoles = selectedRoles ?? new string[] { };


          await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
          await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());


          return Redirect("/admin/user/list");
        }
        return Redirect("/admin/user/list");
      }

      var roles = _roleManager.Roles.Select(i => i.Name);

      ViewBag.Roles = roles;
      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UserDelete(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user != null)
      {
        await _userManager.DeleteAsync(user);
        return Redirect("/admin/user/list");
      }
      return Redirect("/admin/user/list");
    }

    public IActionResult UserList()
    {
      return View(_userManager.Users);
    }

    [HttpPost]
    public async Task<IActionResult> RoleDelete(string RoleId)
    {
      var role = await _roleManager.FindByIdAsync(RoleId);
      if (role != null)
      {
        await _roleManager.DeleteAsync(role);
        return Redirect("/admin/role/list");
      }
      return Redirect("/admin/user/list");
    }

    public async Task<IActionResult> RoleEdit(string id)
    {
      var role = await _roleManager.FindByIdAsync(id);

      var members = new List<User>();
      var nonmembers = new List<User>();

      foreach (var user in _userManager.Users.ToList())
      {
        var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;

        list.Add(user);
      }
      var model = new RoleDetails()
      {
        Role = role,
        Members = members,
        NonMembers = nonmembers
      };
      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RoleEdit(RoleEditModel model)
    {
      foreach (var userId in model.IdsToAdd ?? new string[] { })
      {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
          var result = await _userManager.AddToRoleAsync(user, model.RoleName);
          if (!result.Succeeded)
          {
            foreach (var error in result.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
        }
      }
      foreach (var userId in model.IdsToDelete ?? new string[] { })
      {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
          var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
          if (!result.Succeeded)
          {
            foreach (var error in result.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
        }
      }
      return Redirect("/admin/role/" + model.RoleId);
    }

    public IActionResult RoleList()
    {
      return View(_roleManager.Roles);
    }

    public IActionResult RoleCreate()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> RoleCreate(RoleModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
        if (result.Succeeded)
        {
          return RedirectToAction("RoleList");
        }
        else
        {
          foreach (var error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }

      return View(model);
    }




    public IActionResult ProductList()
    {
      var productList = _productService.GetAll();

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



      return View(new ProductListViewModel()
      {
        Products =  products
      });


    }




    public IActionResult CategoryList()
    {

      return View(new CategoryListViewModel()
      {
        Categories = _categoryService.GetAll()
      });
    }

    public ActionResult ProductCreate()
    {
      return View();
    }

    [HttpPost]
    public ActionResult ProductCreate(ProductModel model)
    {

      var entity = new Product()
      {
        Name = model.Name,
        Price = model.Price,
        Description = model.Description,
        Url = model.Url,
        ImageUrl = model.ImageUrl
      };

      if (_productService.Create(entity))
      {
        TempData.Put("message", new AlertMessage()
        {
          Title = "new product created.",
          Message = "new product created.",
          AlertType = "success"
        });


        return RedirectToAction("ProductList");
      }
      TempData.Put("message", new AlertMessage()
      {
        Title = _productService.ErrorMessage,
        Message = _productService.ErrorMessage,
        AlertType = "danger"
      });


      return View(model);

    }

    public ActionResult CategoryCreate()
    {
      return View();
    }

    [HttpPost]
    public ActionResult CategoryCreate(CategoryModel model)
    {

      var entity = new Category()
      {
        Name = model.Name,
        Url = model.Url
      };

      if (_categoryService.Create(entity))
      {
        TempData.Put("message", new AlertMessage()
        {
          Title = "new category created.",
          Message = "new category created.",
          AlertType = "success"
        });

        return RedirectToAction("CategoryList");
      }
      TempData.Put("message", new AlertMessage()
      {
        Title = _categoryService.ErrorMessage,
        Message = _categoryService.ErrorMessage,
        AlertType = "danger"
      });

      return View(model);

    }

    public IActionResult ProductEdit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var entity = _productService.GetByIdWithCategories((int)id);
      if (entity == null)
      {
        return NotFound();
      }

      var model = new ProductModel()
      {
        ProductId = entity.ProductId,
        Name = entity.Name,
        Price = entity.Price,
        Description = entity.Description,
        Url = entity.Url,
        ImageUrl = entity.ImageUrl,
        IsApproved = entity.IsApproved,
        IsHome = entity.IsHome,

        SelectedCategories = entity.ProductCategories.Select(c => c.Category).ToList()
      };

      ViewBag.Categories = _categoryService.GetAll();

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file)
    {

      var entity = _productService.GetById(model.ProductId);

      if (entity == null)
      {
        return NotFound();
      }

      entity.Name = model.Name;
      entity.Price = model.Price;
      entity.Description = model.Description;
      entity.Url = model.Url;
      entity.IsHome = model.IsHome;
      entity.IsApproved = model.IsApproved;

      if (file != null)
      {
        var extension = Path.GetExtension(file.FileName); // yolladığımız resim formatını tutar.örnek: .jpg , nokta da geliyor
        var randomName = string.Format($"{Guid.NewGuid}{extension}");
        entity.ImageUrl = randomName;
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }
      }

      if (_productService.Update(entity, categoryIds))
      {
        TempData.Put("message", new AlertMessage()
        {
          Title = "Product updated",
          Message = "Product updated",
          AlertType = "success"
        });

        return RedirectToAction("ProductList");
      }
      TempData.Put("message", new AlertMessage()
      {
        Title = _productService.ErrorMessage,
        Message = _productService.ErrorMessage,
        AlertType = "danger"
      });


      ViewBag.Categories = _categoryService.GetAll();
      return View(model);

    }

    public IActionResult CategoryEdit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var entity = _categoryService.GetByIdWithProducts((int)id);
      if (entity == null)
      {
        return NotFound();
      }

      var model = new CategoryModel()
      {
        CategoryId = entity.CategoryId,
        Name = entity.Name,
        Url = entity.Url,
        Products = entity.ProductCategories.Select(p => p.Product).ToList()
      };

      return View(model);
    }

    [HttpPost]
    public IActionResult CategoryEdit(CategoryModel model)
    {


      var entity = _categoryService.GetById(model.CategoryId);

      if (entity == null)
      {
        return NotFound();
      }

      entity.Name = model.Name;
      entity.Url = model.Url;

      if (_categoryService.Update(entity))
      {
        TempData.Put("message", new AlertMessage()
        {
          Title = "Category updated",
          Message = "Category updated",
          AlertType = "success"
        });

        return RedirectToAction("CategoryList");
      }

      TempData.Put("message", new AlertMessage()
      {
        Title = _categoryService.ErrorMessage,
        Message = _categoryService.ErrorMessage,
        AlertType = "danger"
      });


      return View(model);

    }


    [HttpPost]
    public IActionResult DeleteProduct(int productId)
    {
      var entity = _productService.GetById(productId);

      if (entity == null)
      {
        return NotFound();
      }
      _productService.Delete(entity);

      var msg = new AlertMessage()
      {
        Message = $"{entity.Name} Silindi...",
        AlertType = "danger"
      };

      TempData["message"] = JsonConvert.SerializeObject(msg);

      return RedirectToAction("ProductList");
    }

    [HttpPost]
    public IActionResult DeleteCategory(int categoryId)
    {
      var entity = _categoryService.GetById(categoryId);

      if (entity == null)
      {
        return NotFound();
      }
      _categoryService.Delete(entity);

      var msg = new AlertMessage()
      {
        Message = $"{entity.Name} Silindi...",
        AlertType = "danger"
      };

      TempData["message"] = JsonConvert.SerializeObject(msg);

      return RedirectToAction("CategoryList");
    }

    public IActionResult DeleteFromCategory(int productId, int categoryId)
    {
      _categoryService.DeleteFromCategory(productId, categoryId);

      return Redirect("/admin/categories/" + categoryId);
    }


  }
}
