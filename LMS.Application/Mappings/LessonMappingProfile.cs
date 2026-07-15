using AutoMapper;
using LMS.Application.DTOs.Lesson;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public sealed class LessonMappingProfile : Profile
{
    public LessonMappingProfile()
    {
        CreateMap<CreateLessonDto, Lesson>();

        CreateMap<UpdateLessonDto, Lesson>();

        CreateMap<Lesson, LessonResponseDto>();
    }
}