namespace LMS.Application.DTOs.Lesson;

public sealed class UpdateLessonDto
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? VideoUrl { get; set; }

    public int OrderIndex { get; set; }

    public bool IsPublished { get; set; }
}