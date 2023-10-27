using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.BLL.Contracts;

namespace ReportHub.WebApi.controllers;

/// <summary>
/// Controller for managing reports info.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportsInfoController : ControllerBase
{
    private readonly IProjectReportInfoService _projectReportInfoService;

    /// <summary>
    /// Constructor for ReportsInfoController.
    /// </summary>
    public ReportsInfoController(IProjectReportInfoService projectReportInfoService)
    {
        _projectReportInfoService = projectReportInfoService;
    }

    /// <summary>
    /// Get latest project report
    /// </summary>
    [HttpGet("{projectId}/all")]
    public async Task<IActionResult> GetAllrojectReportById([FromRoute] int projectId)
    {
        var result = await _projectReportInfoService.GetAllProjectReportAsync(projectId);

        return Ok(result);
    }
}
