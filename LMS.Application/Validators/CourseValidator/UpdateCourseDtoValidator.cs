using FluentValidation;
using LMS.Application.DTOs.CourseNameSpace;
namespace LMS.Application.Validators.CourseValidator;
public class UpdateCourseDtoValidator:AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator(){
        RuleFor(x=>x.Title)
        .NotEmpty()
        .WithMessage("Course title is required!")
        .MinimumLength(3)
        .WithMessage("Course title must contain at least 3 characters")
        .MaximumLength(200)
        .WithMessage("Course title cannot exceed 200 charcters");

        RuleFor(x=>x.Description)
        .NotEmpty()
        .WithMessage("Course Description is required!")
        .MinimumLength(10)
        .WithMessage("Course Description must contain at least 10 characters")
        .MaximumLength(2000)
        .WithMessage("Course Description cannot exceed 2000 charcters");

        RuleFor(x=>x.Price).GreaterThan(0).WithMessage("course price must be greater then zero");
    }
}