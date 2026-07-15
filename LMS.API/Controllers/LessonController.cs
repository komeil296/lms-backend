using System.Security.Claims;
using LMS.Application.DTOs.Lesson;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/courses/{id:guid}/lessons")]
public sealed class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [Authorize(Policy = "CourseOwner")]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid id,
        CreateLessonDto dto)
    {
        var result = await _lessonService.CreateAsync(
            id,
            dto);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }

    [Authorize(Policy = "CourseOwner")]
    [HttpPut("{lessonId:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        Guid lessonId,
        UpdateLessonDto dto)
    {
        var result = await _lessonService.UpdateAsync(
            id,
            lessonId,
            dto);

        return Ok(result);
    }

    [Authorize(Policy = "CourseOwner")]
    [HttpDelete("{lessonId:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        Guid lessonId)
    {
        await _lessonService.DeleteAsync(
            id,
            lessonId);

        return NoContent();
    }

    [Authorize(Policy = "CourseOwner")]
    [HttpGet("manage")]
    public async Task<IActionResult> GetForTeacher(
        Guid id)
    {
        var result =
            await _lessonService.GetForTeacherAsync(id);

        return Ok(result);
    }

    [Authorize(Roles = "Student")]
    [HttpGet]
    public async Task<IActionResult> GetForStudent(
        Guid id)
    {
        if (!TryGetStudentId(out var studentId))
        {
            return Unauthorized();
        }

        var result =
            await _lessonService.GetForStudentAsync(
                id,
                studentId);

        return Ok(result);
    }

    private bool TryGetStudentId(out Guid studentId)
    {
        var studentIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        return Guid.TryParse(
            studentIdClaim,
            out studentId);
    }
}