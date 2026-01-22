using BackendApi.DTOs;
using BackendApi.Services;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BackendApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudiesController : ControllerBase
{
    private readonly IStudyService _studyService;
    private readonly IUserService _userService;

    public StudiesController(IStudyService studyService, IUserService userService)
    {
        _studyService = studyService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Study>>> GetStudies()
    {
        if (User.IsInRole("Admin"))
        {
            return Ok(await _studyService.GetAllStudiesAsync());
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
        {
            var studies = await _studyService.GetAllStudiesAsync();
            return Ok(studies.Where(s => s.UserId == userId));
        }

        return Forbid();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Study>> GetStudy(int id)
    {
        var study = await _studyService.GetStudyByIdAsync(id);
        if (study == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (User.IsInRole("Admin") || (userIdClaim != null && study.UserId.ToString() == userIdClaim))
        {
            return Ok(study);
        }

        return Forbid();
    }

    [HttpPost]
    public async Task<ActionResult<Study>> CreateStudy(CreateStudyDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && userIdClaim != dto.UserId.ToString())
        {
            return Forbid();
        }

        var study = await _studyService.CreateStudyAsync(dto);
        return CreatedAtAction(nameof(GetStudy), new { id = study.Id }, study);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudy(int id, UpdateStudyDto dto)
    {
        var study = await _studyService.GetStudyByIdAsync(id);
        if (study == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && userIdClaim != study.UserId.ToString())
        {
            return Forbid();
        }

        await _studyService.UpdateStudyAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudy(int id)
    {
        var study = await _studyService.GetStudyByIdAsync(id);
        if (study == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!User.IsInRole("Admin") && userIdClaim != study.UserId.ToString())
        {
            return Forbid();
        }

        await _studyService.DeleteStudyAsync(id);
        return NoContent();
    }
}