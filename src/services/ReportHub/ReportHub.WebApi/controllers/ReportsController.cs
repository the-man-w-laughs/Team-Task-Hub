using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReportHub.WebApi.controllers;

/// <summary>
/// Controller for managing reports.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    /// <summary>
    /// Constructor for ReportsController.
    /// </summary>
    public ReportsController() { }

    /// <summary>
    /// Get new Project
    /// </summary>
    [HttpGet("new")]
    public async Task<IActionResult> GetNewReport()
    {
        return Ok("WOW");
    }
}
