using AutoMapper;
using LMS.Application.Course;
namespace LMS.Application.Mappings;
public class CoursePofile:Profile
{
    public CoursePofile()
    {
        CreateMap<CreateCourseDto,LMS.Domain.Entities.Course>();
    }
}