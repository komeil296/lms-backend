using AutoMapper;
using LMS.Application.DTOs.CourseNameSpace;
using LMS.Domain.Entities;
namespace LMS.Application.Mappings;
public class CourseMappingPofile:Profile
{
    public CourseMappingPofile()
    {
        CreateMap<CreateCourseDto,Course>();
        CreateMap<Course,CourseResonseDto>();
        CreateMap<Course,CourseAuthoriztionDto>();
        CreateMap<UpdateCourseDto,Course>();
    }
}