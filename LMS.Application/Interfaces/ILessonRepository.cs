using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAndCourseAsync( Guid courseId,Guid lessonId);

    Task<IReadOnlyList<Lesson>> GetAllByCourseIdAsync( Guid courseId);

    Task<IReadOnlyList<Lesson>> GetPublishedByCourseIdAsync(Guid courseId);

    Task<bool> OrderIndexExistsAsync(Guid courseId,int orderIndex,Guid? excludedLessonId = null);

    void Add(Lesson lesson);

    void Update(Lesson lesson);

    void Delete(Lesson lesson);

    Task SaveChangesAsync();
}