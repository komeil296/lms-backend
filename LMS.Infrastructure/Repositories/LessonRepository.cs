using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson?> GetByIdAndCourseAsync(
        Guid courseId,
        Guid lessonId)
    {
        return await _context.Lessons
            .FirstOrDefaultAsync(lesson =>
                lesson.Id == lessonId
                && lesson.CourseId == courseId);
    }

    public async Task<IReadOnlyList<Lesson>>
        GetAllByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .AsNoTracking()
            .Where(lesson => lesson.CourseId == courseId)
            .OrderBy(lesson => lesson.OrderIndex)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Lesson>>
        GetPublishedByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .AsNoTracking()
            .Where(lesson =>
                lesson.CourseId == courseId
                && lesson.IsPublished)
            .OrderBy(lesson => lesson.OrderIndex)
            .ToListAsync();
    }

    public async Task<bool> OrderIndexExistsAsync(
        Guid courseId,
        int orderIndex,
        Guid? excludedLessonId = null)
    {
        return await _context.Lessons
            .AnyAsync(lesson =>
                lesson.CourseId == courseId
                && lesson.OrderIndex == orderIndex
                && (!excludedLessonId.HasValue
                    || lesson.Id != excludedLessonId.Value));
    }

    public void Add(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        _context.Lessons.Add(lesson);
    }

    public void Update(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        _context.Lessons.Update(lesson);
    }

    public void Delete(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        _context.Lessons.Remove(lesson);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}