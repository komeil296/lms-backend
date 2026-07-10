using System.Security.Claims;
using AutoMapper.Internal.Mappers;
using LMS.Application.DTOs.CourseNameSpace;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly ICourseService _service;
    public CourseController(ICourseService service)
    {
        _service=service;
    }

    [Authorize(Roles ="Admin,Teacher")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        var teacherId=User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        await _service.CreateAsync(dto,Guid.Parse(teacherId));
        return Ok("course created!");
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        return Ok(await _service.GetAllAsync());
    }
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetyIdAsync(Guid id)
    {
        var result=await _service.GetByIdAsync(id);
        if(result==null) return NotFound();
        return Ok(result);
    }
    [Authorize(Policy ="CourseOwner")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,UpdateCourseDto dto)
    {
       // var userId=User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var updated=await _service.UpdateAsync(id,dto);
        if(!updated) return NotFound();
        return Ok("Course updatetd!");
    }
    [Authorize(Roles ="Teacher")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        //var userId=User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var deleted=await _service.DeleteAsync(id);
        if(!deleted) return NotFound();
        return Ok("Course Deleted!");
    }

}