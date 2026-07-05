using AutoMapper;
using LMS.Application.DTOs.Auth;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;
public class AuthMappingProfile:Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterDto,User>();
    }
}