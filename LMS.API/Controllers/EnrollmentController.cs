using System.Security.Claims;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;
[ApiController]
[Authorize(Roles ="Student")]
[Route("api/enrollments")]
public sealed class EnrollmentController:ControllerBase
{
    private readonly IEnrollService _enrollmentService;
    public EnrollmentController(IEnrollService enrollmentService)
    {
        _enrollmentService=enrollmentService;
    }
    [HttpPost("{courseId:guid}")]
    public async Task<IActionResult> Enroll(Guid courseId)
    {
        if(!TryGetStudentId(out var studentId))
        {
            return Unauthorized();
        }
        var result=await _enrollmentService.EnrollAync(studentId,courseId);
        return StatusCode(StatusCodes.Status201Created,result);
    }
        [HttpGet("my-courses")]
    public async Task<IActionResult> GetMyCourses()
    {
        if (!TryGetStudentId(out var studentId))
        {
            return Unauthorized();
        }

        var result = await _enrollmentService.GetStudentEnrolmentsAsync(studentId);

        return Ok(result);
    }
    [HttpDelete("{courseId:guid}")]
    public async Task<IActionResult> Cancel(Guid courseId)
    {
        if (!TryGetStudentId(out var studentId))
        {
            return Unauthorized();
        }

        await _enrollmentService.CancelAsync(
            studentId,
            courseId);

        return NoContent();
    }
    private bool TryGetStudentId(out Guid studentId)
    {
        var studentIdClaim =User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(
            studentIdClaim,
            out studentId);
    }
}