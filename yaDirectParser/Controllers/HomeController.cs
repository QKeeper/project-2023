using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using yaDirectParser.Middlewares;
using yaDirectParser.Models;

namespace yaDirectParser.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly yaDirectService _yaDirectService;
    
    public HomeController(ILogger<HomeController> logger, yaDirectService yaDirectService)
    {
        _logger = logger;
        _yaDirectService = yaDirectService;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult SuccessfulAuthentication()
    {
        _yaDirectService.InvokeAsync(HttpContext).GetAwaiter().GetResult();
        return RedirectToAction("AdsLayout", "Home");
    }

    public IActionResult AdsLayout()
    {
        return Ok(yaDirectService.Result);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}