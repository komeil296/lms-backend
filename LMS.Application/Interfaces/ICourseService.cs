using LMS.Application.CourseNameSpace;
using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface ICourseService
{
    Task CreateAsync(CreateCourseDto dto);
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(Guid id);
}