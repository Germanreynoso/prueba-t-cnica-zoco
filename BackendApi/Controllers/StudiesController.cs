using BackendApi.DTOs;
using BackendApi.Services;
using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudiesController : ControllerBase
{
    private readonly IStudyService _studyService;

    public StudiesController(IStudyService studyService)
    {
        _studyService = studyService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Study>>> GetStudies()
    {
        var studies = await _studyService.GetAllStudiesAsync();
        return Ok(studies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Study>> GetStudy(int id)
    {
        var study = await _studyService.GetStudyByIdAsync(id);
        if (study == null) return NotFound();
        return Ok(study);
    }

    [HttpPost]
    public async Task<ActionResult<Study>> CreateStudy(CreateStudyDto dto)
    {
        var study = await _studyService.CreateStudyAsync(dto);
        return CreatedAtAction(nameof(GetStudy), new { id = study.Id }, study);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudy(int id, UpdateStudyDto dto)
    {
        await _studyService.UpdateStudyAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudy(int id)
    {
        await _studyService.DeleteStudyAsync(id);
        return NoContent();
    }
}