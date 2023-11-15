using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.BLL.Contracts;

namespace ReportHub.WebApi.Controllers;

/// <summary>
/// Controller for managing reports.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IProjectReportService _projectReportService;

    /// <summary>
    /// Constructor for ReportsController.
    /// </summary>
    public ReportsController(IProjectReportService projectReportInfoService)
    {
        _projectReportService = projectReportInfoService;
    }

    /// <summary>
    /// Get latest project report
    /// </summary>
    [HttpGet("{projectId}/latest")]
    public async Task<IActionResult> GetLatestProjectReportByIdAsync([FromRoute] int projectId)
    {
        var result = await _projectReportService.GetLatestProjectReportAsync(projectId);

        return result;
    }

    /// <summary>
    /// Get project report by file name
    /// </summary>
    [HttpGet("{filename}")]
    public async Task<IActionResult> GetProjectReportByFileNameAsync([FromRoute] string filename)
    {
        var result = await _projectReportService.GetReportByNameAsync(filename);

        return result;
    }

    /// <summary>
    /// Delete project report by file name
    /// </summary>
    [HttpDelete("{filename}")]
    public async Task<IActionResult> DeleteProjectReportByFileNameAsync([FromRoute] string filename)
    {
        var result = await _projectReportService.DeleteReportByNameAsync(filename);

        return Ok(result);
    }
}
