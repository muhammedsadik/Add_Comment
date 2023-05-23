using Business.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.EmailServices;
using MvcWebUI.Extensions;
using MvcWebUI.Identity;
using MvcWebUI.Models;

namespace MvcWebUI.Controllers
{
  public class AccountController : Controller
  {
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private IEmailSender _emailSender;
    private ICartService _cartService;
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _cartService = cartService;
    }

    public IActionResult Login(string ReturnUrl = null)
    {
      return View(new LoginModel()
      {
        ReturnUrl = ReturnUrl
      });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.FindByEmailAsync(model.Email);

      if (user == null)
      {
        ModelState.AddModelError("", "Kullanıcı bulunamadı.");
        return View(model);
      }

      if (!await _userManager.IsEmailConfirmedAsync(user))
      {
        ModelState.AddModelError("", "Lütfen hesabınızı onaylayın.");
        return View(model);
      }

      var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

      if (result.Succeeded)
      {
        return Redirect(model.ReturnUrl ?? "~/");
      }

      ModelState.AddModelError("", "Hatalı giriş.");
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = new User()
      {
        FirstName = model.FirstName,
        LastName = model.LastName,
        UserName = model.UserName,
        Email= model.Email,
        EmailConfirmed= true
        
      };

      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {

        //generate token

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var url = Url.Action("ConfirmEmail", "Account", new
        {
          userId = user.Id,
          token = code
        });

        //await _emailSender.SendEmailAsync( model.Email,
        //  "Hesabınızı onaylayın",
        //  $"<a href'https://localhost:44367{url}'>Lütfen Hesabınızı naylamak için tıklayınız.</a>"
        //  );

        _cartService.InitializeCart(user.Id);
        return RedirectToAction("Login", "Account");
      }

      ModelState.AddModelError("", "Bilinmeyen hata!  Lütfen tekrar deneyiniz.");

      return View(model);
    }

    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();

      TempData.Put("message", new AlertMessage()
      {
        Title = "session closed",
        Message = "the session successfully closed",
        AlertType = "success"
      });

      return Redirect("~/");
    }

    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
      if (userId == null || token == null)
      {

        TempData.Put("message", new AlertMessage()
        {
          Title= "Hatalı işlem.",
          Message="Geçersiz Token",
          AlertType = "danger"
        });

        return View();
      }

      var user = await _userManager.FindByIdAsync(userId);

      if (user != null)
      {
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
          //_cartService.InitializeCart(user.Id);

          TempData.Put("message", new AlertMessage()
          {
            Title = "Hesabınız onaylandı.",
            Message = "Hesabınız onaylandı.",
            AlertType = "success"
          });

          return View();
        }

      }
      TempData.Put("message", new AlertMessage()
      {
        Title = "Hesabınız onaylanmadı.",
        Message = "Hesabınız onaylanmadı.",
        AlertType = "warning"
      });

      return View();
    }

    public IActionResult ForgatPassword()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgatPassword(string Email)
    {
      if (string.IsNullOrEmpty(Email))
      {
        return View();
      }

      var user = await _userManager.FindByEmailAsync(Email);
      if (user == null)
      {
        return View();
      }

      var code = await _userManager.GeneratePasswordResetTokenAsync(user);
      var url = Url.Action("ResetPassword", "Account", new
      {
        userId = user.Id,
        token = code
      });

      await _emailSender.SendEmailAsync(
        Email,
        "Reset Password",
        $"<a href'https://localhost:44367{url}'>Parolanızı yenilemek için tıklayınız.</a>"
        );

      return View();

    }

    public IActionResult ResetPassword(string userId, string token)
    {
      if (User == null || token == null)
      {
        return Redirect("~/");
      }

      var model = new ResetPasswordModel { Token= token };

      return View(model);

    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
      if (ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.FindByEmailAsync(model.Email);
      if(user==null)
      {
        return Redirect("~/");
      }
      var result = await _userManager.ResetPasswordAsync(user, model.Token,model.Password);

      if (result.Succeeded)
        return RedirectToAction("Login","Account");

      return View(model);
    }



    public IActionResult accessdenied()
    {
      return View();
    }




  }
}
