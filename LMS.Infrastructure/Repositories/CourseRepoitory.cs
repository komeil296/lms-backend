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
    public void Add(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);
         _context.Courses.Add(course);
    }
    public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Courses.AsNoTracking().Include(x=>x.Teacher).ToListAsync();
    }
    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses.Include(x=>x.Teacher).FirstOrDefaultAsync(x=>x.Id==id);
    }
    public void Update(Course course)
    {
        _context.Courses.Update(course);
    }

    public void Delete(Course course)
    {
        _context.Courses.Remove(course);
    }
    public async Task SavechangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}