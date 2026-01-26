using BackendApi.Models;
using BackendApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class SessionLogsController : ControllerBase
{
    private readonly SessionService _sessionService;

    public SessionLogsController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessionLog>>> GetLogs()
    {
        var logs = await _sessionService.GetAllSessionLogsAsync();
        return Ok(logs);
    }
}
