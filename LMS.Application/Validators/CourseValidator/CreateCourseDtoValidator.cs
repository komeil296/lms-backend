using LMS.Application.DTOs.CourseNameSpace;
using FluentValidation;
namespace LMS.Application.Validators.CourseValidator;
public class CreateCourseDtoValidator:AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(x=>x.Description).NotEmpty().MinimumLength(10);

        RuleFor(x=>x.Price).GreaterThan(0);
    }
}