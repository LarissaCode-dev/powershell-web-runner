using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAppToExecutePowershell.Models;
using WebAppToExecutePowershell.Services;
using System.Text.Json;

namespace WebAppToExecutePowershell.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PowerShellService _powerShellService;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _powerShellService = new PowerShellService(webHostEnvironment);
    }

    public async Task<IActionResult> Index(string? processName = null, bool search = false)
    {
        var model = new ProcessSearchModel { ProcessName = processName };

        if (!search)
        {
            return View(model);
        }

        try
        {
            model.Results = await _powerShellService.ExecutePowerShellScript("GetProcesses.ps1", processName);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing PowerShell script");
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Index(ProcessSearchModel model)
    {
        return RedirectToAction(nameof(Index), new { processName = model.ProcessName, search = true });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
