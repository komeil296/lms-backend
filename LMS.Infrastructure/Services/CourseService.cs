using AutoMapper;
using LMS.Application.DTOs.CourseNameSpace;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Repositories;

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

         _courseRepository.Add(course);
        await _courseRepository.SavechangeAsync();
    }
    public async Task<List<CourseResonseDto>> GetAllAsync()
    {
        var courses= await _courseRepository.GetAllAsync();
        return _mapper.Map<List<CourseResonseDto>>(courses);
    }
    public async Task<CourseResonseDto?> GetByIdAsync(Guid id)
    {
        var course= await _courseRepository.GetByIdAsync(id);
        return _mapper.Map<CourseResonseDto>(course);
    }
    public async Task<bool> UpdateAsync(Guid id,UpdateCourseDto dto)
    {
        var course=await _courseRepository.GetByIdAsync(id);
        if(course==null) return false;
        _mapper.Map(dto,course);
        _courseRepository.Update(course);
        await _courseRepository.SavechangeAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var course=await _courseRepository.GetByIdAsync(id);
        if(course==null) return false;
         _courseRepository.Delete(course);
        await _courseRepository.SavechangeAsync();
        return true;
    }
}