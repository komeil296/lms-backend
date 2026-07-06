using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;
public class CourseRepoitory:ICourseRepository
{
    private readonly AppDbContext _context;
    public CourseRepoitory(AppDbContext context)
    {
        _context=context;
    }
    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
    }
    public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }
    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses.FindAsync(id);
    }
    public async Task SavechangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}