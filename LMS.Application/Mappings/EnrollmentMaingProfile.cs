using AutoMapper;
using LMS.Application.DTOs.Enrollment;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;
public sealed class EnrollmentMaingProfile:Profile
{
    public EnrollmentMaingProfile()
    {
        CreateMap<Enrollment,EnrollmentResponseDto>()
        .ForMember(
            destinaton=>destinaton.CourseTitle,
            options=>options.MapFrom(source=>source.Course.Title)
        )
        .ForMember(
            destination=>destination.TeacherName,
            options=>options.MapFrom(
                source=>source.Course.Teacher.Username
            )
        )
        .ForMember(
            destination=>destination.Price,
            options=>options.MapFrom(
                source=>source.Course.Price
            )
        );
    }
    
}