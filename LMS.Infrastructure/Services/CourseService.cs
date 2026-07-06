using AutoMapper;
using LMS.Application.CourseNameSpace;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Services;
public class CourseService:ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository,IMapper mapper)
    {
        _courseRepository=courseRepository;
        _mapper=mapper;
    }
    public async Task CreateAsync(CreateCourseDto dto)
    {
        var course=_mapper.Map<Course>(dto);
        course.Id=Guid.NewGuid();
        course.CreatedAt=DateTime.UtcNow;

        await _courseRepository.AddAsync(course);
        await _courseRepository.SavechangeAsync();
    }
    public async Task<List<Course>> GetAllAsync()
    {
        return await _courseRepository.GetAllAsync();
    }
    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _courseRepository.GetByIdAsync(id);
    }
}