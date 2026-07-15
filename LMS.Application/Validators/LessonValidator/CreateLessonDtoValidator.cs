using FluentValidation;
using LMS.Application.DTOs.Lesson;

namespace LMS.Application.Validators.LessonValidator;

public sealed class CreateLessonDtoValidator
    : AbstractValidator<CreateLessonDto>
{
    public CreateLessonDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Lesson title is required.")
            .MinimumLength(3)
            .WithMessage(
                "Lesson title must contain at least 3 characters.")
            .MaximumLength(200)
            .WithMessage(
                "Lesson title cannot exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Lesson content is required.")
            .MinimumLength(10)
            .WithMessage(
                "Lesson content must contain at least 10 characters.")
            .MaximumLength(50000)
            .WithMessage(
                "Lesson content cannot exceed 50000 characters.");

        RuleFor(x => x.OrderIndex)
            .GreaterThan(0)
            .WithMessage(
                "Lesson order must be greater than zero.");

        RuleFor(x => x.VideoUrl)
            .MaximumLength(2048)
            .WithMessage(
                "Video URL cannot exceed 2048 characters.")
            .Must(BeAValidVideoUrl)
            .WithMessage(
                "Video URL must be a valid HTTP or HTTPS address.")
            .When(x =>
                !string.IsNullOrWhiteSpace(x.VideoUrl));
    }

    private static bool BeAValidVideoUrl(string? videoUrl)
    {
        return Uri.TryCreate(
                   videoUrl,
                   UriKind.Absolute,
                   out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp
                   || uri.Scheme == Uri.UriSchemeHttps);
    }
}