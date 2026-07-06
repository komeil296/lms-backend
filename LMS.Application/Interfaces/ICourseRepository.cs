using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface ICourseRepository
{
    Task AddAsync(Course course);
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(Guid id);
    Task SavechangeAsync();
}