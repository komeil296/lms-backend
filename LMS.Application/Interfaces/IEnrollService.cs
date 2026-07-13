

using LMS.Application.DTOs.Enrollment;

namespace LMS.Application.Interfaces;
public interface IEnrollService
{
    Task<EnrollmentResponseDto> EnrollAync(Guid studentId,Guid courseId);
    Task<IReadOnlyList<EnrollmentResponseDto>> GetStudentEnrolmentsAsync(Guid studentId);
    Task CancelAsync(Guid studentId,Guid courseId);
}