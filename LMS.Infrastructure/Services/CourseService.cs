using AutoMapper;
using LMS.Application.DTOs.CourseNameSpace;
using LMS.Application.Exceptions;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace LMS.Infrastructure.Services;
public class CourseService:ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CourseService> _logger;

    public CourseService(ICourseRepository courseRepository,IMapper mapper,ILogger<CourseService> logger)
    {
        _courseRepository=courseRepository;
        _mapper=mapper;
        _logger=logger;
    }
    public async Task CreateAsync(CreateCourseDto dto,Guid teacherId)
    {
        var course=_mapper.Map<Course>(dto);
        course.TeacherId=teacherId;
        course.Id=Guid.NewGuid();
        course.CreatedAt=DateTime.UtcNow;

         _courseRepository.Add(course);
        await _courseRepository.SavechangeAsync();
        _logger.LogInformation("Course created succesfully CourseId:{CourseID}",course.Id);
    }
    public async Task<List<CourseResonseDto>> GetAllAsync()
    {
        var courses= await _courseRepository.GetAllAsync();
        return _mapper.Map<List<CourseResonseDto>>(courses);
    }
    public async Task<CourseAuthoriztionDto?> GetByIdAsyncForAuthorization(Guid id)
    {
         var course= await _courseRepository.GetByIdAsync(id);
        return _mapper.Map<CourseAuthoriztionDto>(course);
    }
    public async Task<CourseResonseDto?> GetByIdAsync(Guid id)
    {
        var course= await _courseRepository.GetByIdAsync(id);
        return _mapper.Map<CourseResonseDto>(course);
    }
    public async Task<bool> UpdateAsync(Guid id,UpdateCourseDto dto)
    {
        var course=await _courseRepository.GetByIdAsync(id);
        if(course==null)  throw new NotFoundException("Course not Found");
       // if(course.TeacherId!=userId)throw new UnauthorizedAccessException();
        _mapper.Map(dto,course);
        _courseRepository.Update(course);
        await _courseRepository.SavechangeAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var course=await _courseRepository.GetByIdAsync(id);
        if(course==null) throw new NotFoundException("Course not Found");
        //if(course.TeacherId!=userId)throw new UnauthorizedAccessException();
         _courseRepository.Delete(course);
        await _courseRepository.SavechangeAsync();
        return true;
    }
}