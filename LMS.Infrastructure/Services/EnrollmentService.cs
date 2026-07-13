using AutoMapper;
using LMS.Application.DTOs.Enrollment;
using LMS.Application.Exceptions;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;
public sealed class EnrollmentService:IEnrollService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EnrollmentService> _logger;
    public EnrollmentService(IEnrollmentRepository enrollmentRepository,ICourseRepository courseRepository,IMapper mapper,ILogger<EnrollmentService> logger)
    {
        _enrollmentRepository=enrollmentRepository;
        _courseRepository=courseRepository;
        _mapper=mapper;
        _logger=logger;
    }

    public async Task<EnrollmentResponseDto> EnrollAync(Guid studentId,Guid courseId)
    {
        var course=await _courseRepository.GetByIdAsync(courseId);
        if(course is null)
        {
            throw new NotFoundException("Course not found");
        }
        var existingEnrollment=await _enrollmentRepository.GetByStudentAndCourseAsync(studentId,courseId);

        if(existingEnrollment is not null)
         {
            if (existingEnrollment.Status == EnrollmentStatus.Active)
            {
                throw new ConflictExpecion("Student is already enrolled in this course.");
            }
              if (existingEnrollment.Status == EnrollmentStatus.Completed)
            {
                throw new ConflictExpecion("A completed enrollment cannot be reactivated.");
            }

            existingEnrollment.Status = EnrollmentStatus.Active;
            existingEnrollment.EnrolledAt = DateTime.UtcNow;

            _enrollmentRepository.update(existingEnrollment);
            await _enrollmentRepository.SaveChangeAsync();

            _logger.LogInformation(
                "Enrollment reactivated. StudentId: {StudentId}, CourseId: {CourseId}",
                studentId,
                courseId);

            return _mapper.Map<EnrollmentResponseDto>(existingEnrollment);
        }
        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId,
            Course = course,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active
        };

        _enrollmentRepository.Add(enrollment);
        await _enrollmentRepository.SaveChangeAsync();

        _logger.LogInformation(
            "Student enrolled. StudentId: {StudentId}, CourseId: {CourseId}",
            studentId,
            courseId);

        return _mapper.Map<EnrollmentResponseDto>(enrollment);
    }

    public async Task<IReadOnlyList<EnrollmentResponseDto>> GetStudentEnrolmentsAsync(Guid studentId)
    {
        var enrollments=await _enrollmentRepository.GetByStudentIdAsync(studentId);
        return _mapper.Map<List<EnrollmentResponseDto>>(enrollments);
    }

    public async Task CancelAsync(Guid studentId,Guid courseId)
    {
        var enrollment=await _enrollmentRepository.GetByStudentAndCourseAsync(studentId,courseId);

        if(enrollment is null)
        {
            throw new NotFoundException("Enrollment not Found!");
        }

        if (enrollment.Status == EnrollmentStatus.Cancelled)
        {
            return;
        }

        if (enrollment.Status == EnrollmentStatus.Completed)
        {
            throw new ConflictExpecion("A compeleted enrollment cannot be cancelled");
        }

        enrollment.Status=EnrollmentStatus.Cancelled;
        _enrollmentRepository.update(enrollment);

        await _enrollmentRepository.SaveChangeAsync();
        _logger.LogInformation("Enrollment canceled . StudentId : {StudentId} , CourseId : {CourseId}",studentId,courseId);
    }
}