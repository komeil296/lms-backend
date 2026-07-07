using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface ICourseRepository
{
    void Add(Course course);
    void Update(Course course);
    void Delete(Course course);
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(Guid id);
    Task SavechangeAsync();
}