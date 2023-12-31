using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.BLL.Contracts;

namespace ReportHub.WebApi.Controllers;

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
    /// Get all project reports
    /// </summary>
    [HttpGet("{projectId}/all")]
    public async Task<IActionResult> GetAllrojectReportByIdAsync(
        [FromRoute] int projectId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var result = await _projectReportInfoService.GetAllProjectReportsAsync(
            projectId,
            offset,
            limit
        );

        return Ok(result);
    }
}
