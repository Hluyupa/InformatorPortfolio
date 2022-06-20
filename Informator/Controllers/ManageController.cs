using Informator.Models;
using Informator.Models.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Informator.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public class ManageController : Controller {
    private readonly ILogger _logger;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;

    public ManageController(ILogger<ManageController> logger, UserManager<UserIdentity> userManager, SignInManager<UserIdentity> signInManager) {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index() {
        var user = await _userManager.GetUserAsync(User);

        if (user is null) {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model) {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {
        throw new NotImplementedException();
    }
}
