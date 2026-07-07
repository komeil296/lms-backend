using FluentValidation;
using LMS.Application.DTOs.CourseNameSpace;
namespace LMS.Application.Validators.CourseValidator;
public class UpdateCourseDtoValidator:AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator(){
        RuleFor(x=>x.Title).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(x=>x.Description).NotEmpty().MinimumLength(10);

        RuleFor(x=>x.Price).GreaterThan(0);
    }
}