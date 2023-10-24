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
public class ProjectReportInfoController : ControllerBase
{
    private readonly IProjectReportInfoService _projectReportInfoService;

    /// <summary>
    /// Constructor for ReportsInfoController.
    /// </summary>
    public ProjectReportInfoController(IProjectReportInfoService projectReportInfoService)
    {
        _projectReportInfoService = projectReportInfoService;
    }

    /// <summary>
    /// Get all users project info
    /// </summary>
    [HttpGet("local")]
    public async Task<IActionResult> GetAllProjectInfo()
    {
        var result = await _projectReportInfoService.GetAllUsersProjectReportInfo();
        return Ok(result);
    }

    /// <summary>
    /// Get all users project info
    /// </summary>
    [HttpGet("{projectId}/external")]
    public async Task<IActionResult> GetAllProjectByIdInfo([FromRoute] int projectId)
    {
        var result = await _projectReportInfoService.GetProjectsDataById(projectId);
        return Ok(result);
    }
}
