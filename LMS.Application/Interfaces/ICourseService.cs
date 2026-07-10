using LMS.Application.DTOs.CourseNameSpace;
using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface ICourseService
{
    Task CreateAsync(CreateCourseDto dto,Guid teacherId);
    Task<List<CourseResonseDto>> GetAllAsync();
    Task<CourseResonseDto?> GetByIdAsync(Guid id);
    Task<CourseAuthoriztionDto?> GetByIdAsyncForAuthorization(Guid id);
    Task<bool> UpdateAsync(Guid id,UpdateCourseDto dto);
    Task<bool> DeleteAsync(Guid id);
}