namespace LMS.Application.DTOs.Lesson;
public sealed class LessonResponseDto
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? VideoUrl { get; set; }

    public int OrderIndex { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }
}