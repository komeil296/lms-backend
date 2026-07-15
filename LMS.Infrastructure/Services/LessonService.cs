using AutoMapper;
using LMS.Application.DTOs.Lesson;
using LMS.Application.Exceptions;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public sealed class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<LessonService> _logger;

    public LessonService(
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        IMapper mapper,
        ILogger<LessonService> logger)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<LessonResponseDto> CreateAsync(
        Guid courseId,
        CreateLessonDto dto)
    {
        var course =
            await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
        {
            throw new NotFoundException("Course not found.");
        }

        var orderExists =
            await _lessonRepository.OrderIndexExistsAsync(
                courseId,
                dto.OrderIndex);

        if (orderExists)
        {
            throw new ConflictExpecion(
                "Another lesson already uses this order.");
        }

        var lesson = _mapper.Map<Lesson>(dto);

        lesson.Id = Guid.NewGuid();
        lesson.CourseId = courseId;
        lesson.CreatedAt = DateTime.UtcNow;

        _lessonRepository.Add(lesson);
        await _lessonRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Lesson created. LessonId: {LessonId}, CourseId: {CourseId}",
            lesson.Id,
            courseId);

        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task<LessonResponseDto> UpdateAsync(
        Guid courseId,
        Guid lessonId,
        UpdateLessonDto dto)
    {
        var lesson =
            await _lessonRepository.GetByIdAndCourseAsync(
                courseId,
                lessonId);

        if (lesson is null)
        {
            throw new NotFoundException("Lesson not found.");
        }

        var orderExists =
            await _lessonRepository.OrderIndexExistsAsync(
                courseId,
                dto.OrderIndex,
                lessonId);

        if (orderExists)
        {
            throw new ConflictExpecion(
                "Another lesson already uses this order.");
        }

        _mapper.Map(dto, lesson);

        _lessonRepository.Update(lesson);
        await _lessonRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Lesson updated. LessonId: {LessonId}, CourseId: {CourseId}",
            lesson.Id,
            courseId);

        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task DeleteAsync(
        Guid courseId,
        Guid lessonId)
    {
        var lesson =
            await _lessonRepository.GetByIdAndCourseAsync(
                courseId,
                lessonId);

        if (lesson is null)
        {
            throw new NotFoundException("Lesson not found.");
        }

        _lessonRepository.Delete(lesson);
        await _lessonRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Lesson deleted. LessonId: {LessonId}, CourseId: {CourseId}",
            lesson.Id,
            courseId);
    }

    public async Task<IReadOnlyList<LessonResponseDto>>
        GetForTeacherAsync(Guid courseId)
    {
        var course =
            await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
        {
            throw new NotFoundException("Course not found.");
        }

        var lessons =
            await _lessonRepository.GetAllByCourseIdAsync(
                courseId);

        return _mapper.Map<List<LessonResponseDto>>(lessons);
    }

    public async Task<IReadOnlyList<LessonResponseDto>>
        GetForStudentAsync(
            Guid courseId,
            Guid studentId)
    {
        var course =
            await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
        {
            throw new NotFoundException("Course not found.");
        }

        var enrollment =
            await _enrollmentRepository
                .GetByStudentAndCourseAsync(
                    studentId,
                    courseId);

        if (enrollment is null
            || enrollment.Status == EnrollmentStatus.Cancelled)
        {
            throw new ForbiddenException(
                "You do not have access to this course.");
        }

        var lessons =
            await _lessonRepository
                .GetPublishedByCourseIdAsync(courseId);

        return _mapper.Map<List<LessonResponseDto>>(lessons);
    }
}