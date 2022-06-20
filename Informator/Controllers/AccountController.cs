using Informator.Models;
using Informator.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Informator.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public class AccountController : Controller {
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;

    public AccountController(ILogger<AccountController> logger, UserManager<UserIdentity> userManager, SignInManager<UserIdentity> signInManager) {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login() {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register() {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model) {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user is null || await _userManager.CheckPasswordAsync(user, model.Password) is false) {
            return Unauthorized();
        }

        await _signInManager.SignInAsync(user, model.RememberMe);

        return Redirect("/");
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model) {
        if (await _userManager.FindByNameAsync(model.Username) is not null
            || await _userManager.FindByEmailAsync(model.Email) is not null) {
            return View(BadRequest());
        }

        var user = new UserIdentity() {
            Email = model.Email,
            UserName = model.Username,
            DataUser = new DataUser() {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Email = model.Email
            }
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded) {
            var loging = new LoginViewModel() {
                Username = model.Username,
                Password = model.Password,
                RememberMe = false
            };
            return await Login(loging);
        }
        else {
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> LogOut() {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}