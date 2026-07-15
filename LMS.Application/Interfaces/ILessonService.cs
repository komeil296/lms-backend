using LMS.Application.DTOs.Lesson;

namespace LMS.Application.Interfaces;

public interface ILessonService
{
    Task<LessonResponseDto> CreateAsync(Guid courseId,CreateLessonDto dto);

    Task<LessonResponseDto> UpdateAsync( Guid courseId,Guid lessonId,UpdateLessonDto dto);

    Task DeleteAsync( Guid courseId,Guid lessonId);

    Task<IReadOnlyList<LessonResponseDto>> GetForTeacherAsync(Guid courseId);

    Task<IReadOnlyList<LessonResponseDto>> GetForStudentAsync(Guid courseId,Guid studentId);
}