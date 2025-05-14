using System.Diagnostics;
using CleanKit.Net.Template.Presentation.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanKit.Net.Template.Presentation.Mvc.Controllers;

public class ErrorsController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error404()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error500()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}