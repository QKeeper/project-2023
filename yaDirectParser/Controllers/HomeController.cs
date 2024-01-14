using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using yaDirectParser.Middlewares;
using yaDirectParser.Models;

namespace yaDirectParser.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private IMapper _mapper;

    private readonly yaDirectService _yaDirectService;
    
    public HomeController(ILogger<HomeController> logger, yaDirectService yaDirectService, IMapper mapper)
    {
        _logger = logger;
        _yaDirectService = yaDirectService;
        _mapper = mapper;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    [EnableCors("FreeForAllPolicy")]
    [HttpGet]
    [Route("authentication")]
    public async Task SuccessfulAuthentication()
    {
        var token = HttpContext.Request.Headers.Authorization;
        HttpContext.Session.SetString("AccessToken", token);
        await _yaDirectService.InvokeAsync(HttpContext);
        
    }
    [EnableCors("FreeForAllPolicy")]
    [HttpGet]
    [Route("layout")]
    public async Task AdsLayout()
    {
        var ads = yaDirectService.Result?.ToArray();
        var adViewModel = _mapper.Map<Ad[], AdViewModel[]>(ads);
        await HttpContext.Response.WriteAsJsonAsync(AdViewModel.SortByDate(adViewModel));
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}