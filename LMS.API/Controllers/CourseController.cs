using LMS.Application.DTOs.CourseNameSpace;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly ICourseService _service;
    public CourseController(ICourseService service)
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

        return Ok(await _service.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetyIdAsync(Guid id)
    {
        var result=await _service.GetByIdAsync(id);
        if(result==null) return NotFound();
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,UpdateCourseDto dto)
    {
        var updated=await _service.UpdateAsync(id,dto);
        if(!updated) return NotFound();
        return Ok("Course updatetd!");
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted=await _service.DeleteAsync(id);
        if(!deleted) return NotFound();
        return Ok("Course Deleted!");
    }

}