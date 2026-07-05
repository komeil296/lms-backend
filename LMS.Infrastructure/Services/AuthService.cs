using AutoMapper;
using Azure.Core;
using LMS.Application.DTOs;
using LMS.Application.DTOs.Auth;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;
public class AuthService:IAUthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;
    private readonly ITokenService _tokenService;
    public AuthService(IUserRepository userRepository,IPasswordService passwordSrvice,IMapper mapper,ILogger<AuthService> logger,ITokenService tokenService)
    {
        _userRepository=userRepository;
        _passwordService=passwordSrvice;
        _mapper=mapper;
        _logger=logger;
        _tokenService=tokenService;
    }
     public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        var existing=await _userRepository.GetByUSernameAsync(dto.Username);
        if (existing != null) return false;
        var user=_mapper.Map<User>(dto);
        user.PasswordHash=_passwordService.HashPassword(dto.Password);
        user.Role=UserRole.Student;
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;

    }
     
    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user=await _userRepository.GetByUSernameAsync(dto.Username);
        if(user==null) return null;
        if(!_passwordService.VerifyPassword(dto.Password,user.PasswordHash)) return null;
        
    
        var accessToken=_tokenService.CreateToken(user);
        var refreshToken=_tokenService.GenerateRefreshToken();
        user.RefreshToken=refreshToken;
        user.RefreshTokenExpiryTime=DateTime.UtcNow.AddDays(7);

        await _userRepository.SaveChangesAsync();
        return new AuthResponseDto
        {
            AccessToken=accessToken,
            RefreshToken=refreshToken
        };

    }
    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var user=await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if(user==null) return null;
        if(user.RefreshTokenExpiryTime<DateTime.UtcNow) return null;
        
        var newAccessToken=_tokenService.CreateToken(user);
        var newRefreshToken=_tokenService.GenerateRefreshToken();
        user.RefreshToken=newRefreshToken;
        user.RefreshTokenExpiryTime=DateTime.UtcNow.AddDays(7);
        await _userRepository.SaveChangesAsync();
        return new AuthResponseDto
        {
            AccessToken=newAccessToken,
            RefreshToken=newRefreshToken
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var user=await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if(user==null) return false;
        user.RefreshToken=null;
        user.RefreshTokenExpiryTime=null;
        await _userRepository.SaveChangesAsync();
        return true;


    }
}