using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.BLL.Contracts;

namespace ReportHub.WebApi.Controllers;

/// <summary>
/// Controller for managing projects info.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsInfoController : ControllerBase
{
    private readonly IProjectInfoService _projectInfoService;

    /// <summary>
    /// Constructor for ProjectsInfoController.
    /// </summary>
    public ProjectsInfoController(IProjectInfoService projectInfoService)
    {
        _projectInfoService = projectInfoService;
    }

    /// <summary>
    /// Get all awailable user project infos
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUserProjectInfosAsync(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 100
    )
    {
        var result = await _projectInfoService.GetAllUserProjectInfosAsync(offset, limit);

        return Ok(result);
    }
}
