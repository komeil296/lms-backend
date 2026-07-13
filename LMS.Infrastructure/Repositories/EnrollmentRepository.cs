using System.Runtime.InteropServices;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;
public sealed class EnrollmentRepository:IEnrollmentRepository
{
    private readonly AppDbContext _context;
    public EnrollmentRepository(AppDbContext context)
    {
        _context=context;
    }
    public async Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId,Guid courseId)
    {
        return await _context.Enrollments.Include(e=>e.Course).ThenInclude(c=>c.Teacher).FirstOrDefaultAsync(e=>e.StudentId==studentId && e.CourseId==courseId);
    }
    public async Task<IReadOnlyList<Enrollment>> GetByStudentIdAsync(Guid studentId)
    {
        return await _context.Enrollments.AsNoTracking().Where(e=>e.StudentId==studentId).Include(e=>e.Course).ThenInclude(c=>c.Teacher).OrderByDescending(e=>e.EnrolledAt).ToListAsync();
    }
    public void Add(Enrollment enrollment)
    {
        ArgumentNullException.ThrowIfNull(enrollment);
        _context.Enrollments.Add(enrollment);
    }
    public void update(Enrollment enrollment)
    {
        ArgumentNullException.ThrowIfNull(enrollment);
        _context.Enrollments.Update(enrollment);
    }
    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}