using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace yaDirectParser.Controllers;

public class AccountController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    /*[EnableCors("FreeForAllPolicy")]
    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/home/SuccessfulAuthentication" }, "Yandex");
    }*/
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}