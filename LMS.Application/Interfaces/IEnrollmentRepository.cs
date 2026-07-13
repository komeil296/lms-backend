using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId,Guid courseId);
    Task<IReadOnlyList<Enrollment>> GetByStudentIdAsync(Guid studentId);
    void Add(Enrollment enrollment);
    void update(Enrollment enrollment);
    Task SaveChangeAsync();
}