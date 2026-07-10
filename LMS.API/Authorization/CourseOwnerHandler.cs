using System.Security.Claims;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace LMS.API.Authorization;
public class CourseOwnerHandler:AuthorizationHandler<CourseOwnerRequirement>
{
    private readonly ICourseService _courseService;
    public CourseOwnerHandler(ICourseService courseService)
    {
        _courseService=courseService;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CourseOwnerRequirement requirement)
    {
       // return Task.CompletedTask;
       var role=context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (role == "Admin")
        {
            context.Succeed(requirement);
            return;
        }

        var userIdClaim=context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userIdClaim==null) return;

        var userId=Guid.Parse(userIdClaim);

        if(context.Resource is HttpContext httpContext)
        {
            var courseId=httpContext.Request.RouteValues["id"]?.ToString();
            if(Guid.TryParse(courseId,out var id))
            {
                var coursedto=await _courseService.GetByIdAsyncForAuthorization(id);
                if(coursedto is not null && coursedto.TeacherId == userId)
                {
                    context.Succeed(requirement);
                }
            }
        }


    }
}