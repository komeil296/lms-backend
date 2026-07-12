using AutoMapper;
using LMS.Application.DTOs.CourseNameSpace;
using LMS.Domain.Entities;
namespace LMS.Application.Mappings;
public class CourseMappingPofile:Profile
{
    public CourseMappingPofile()
    {
        CreateMap<CreateCourseDto,Course>();
        CreateMap<Course,CourseResonseDto>().ForMember(
            dest=>dest.TeacherName,opt=>opt.MapFrom(src=>src.Teacher.Username)
        );
        CreateMap<Course,CourseAuthoriztionDto>();
        CreateMap<UpdateCourseDto,Course>();
    }
}