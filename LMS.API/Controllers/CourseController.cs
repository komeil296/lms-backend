using LMS.Application.CourseNameSpace;
using LMS.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly CourseService _service;
    public CourseController(CourseService service)
    {
        _service=service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok("course created!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result=await _service.GetAllAsync();
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetyIdAsync(Guid id)
    {
        var course=await _service.GetByIdAsync(id);
        if(course==null) return NotFound();
        return Ok(course);
    }

}