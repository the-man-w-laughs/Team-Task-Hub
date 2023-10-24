using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.BLL.Contracts;

namespace ReportHub.WebApi.controllers;

/// <summary>
/// Controller for managing reports.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IProjectReportInfoService _projectReportInfoService;

    /// <summary>
    /// Constructor for ReportsController.
    /// </summary>
    public ReportsController(IProjectReportInfoService projectReportInfoService)
    {
        this._projectReportInfoService = projectReportInfoService;
    }

    /// <summary>
    /// Get latest project report
    /// </summary>
    [HttpGet("{projectId}/latest")]
    public async Task<IActionResult> GetLatestProjectReportById([FromRoute] int projectId)
    {
        var result = await _projectReportInfoService.GetLatestProjectReportById(projectId);
        return Ok(result);
    }
}
