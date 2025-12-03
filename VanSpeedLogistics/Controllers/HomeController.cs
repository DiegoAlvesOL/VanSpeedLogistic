using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanSpeedLogistics.Models;


[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    public IActionResult Index()
    {
        if (User.IsInRole("Manager"))
        {
            return RedirectToAction("Index", "Manager");
        }
        
        if (User.IsInRole("Operator"))
        {
            return RedirectToAction("Index", "Operator");
        }
        
        return RedirectToAction("Index", "Manager");
    }

    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}