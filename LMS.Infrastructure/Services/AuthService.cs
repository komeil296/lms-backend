using System.Security.Cryptography;
using System.Text;
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
    private static string HashRefreshToken(string refreshToken)
    {
        
        ArgumentException.ThrowIfNullOrWhiteSpace( refreshToken);

        var tokenBytes =Encoding.UTF8.GetBytes(refreshToken);

        var hashBytes =SHA256.HashData(tokenBytes);

        return Convert.ToHexString(hashBytes);
    }
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
        user.PasswordHash=_passwordService.HashPassword(user,dto.Password);
        user.Role=UserRole.Student;
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;

    }
     
    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user=await _userRepository.GetByUSernameAsync(dto.Username);
        if(user==null) return null;
        if(!_passwordService.VerifyPassword(user,dto.Password,user.PasswordHash)) return null;
        
    
        var accessToken=_tokenService.CreateToken(user);
        var refreshToken=_tokenService.GenerateRefreshToken();

        user.RefreshTokenHash=HashRefreshToken(refreshToken);
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
        if(string.IsNullOrWhiteSpace(refreshToken)) return null;

        var refreshTokenHash=HashRefreshToken(refreshToken);
        var user=await _userRepository.GetByRefreshTokenHashAsync(refreshToken);

        if(user is null) return null;
        if(user.RefreshTokenExpiryTime is null ||user.RefreshTokenExpiryTime<=DateTime.UtcNow) return null;

        var newAccessToken=_tokenService.CreateToken(user);
        var newRefreshToken=_tokenService.GenerateRefreshToken();

        user.RefreshTokenHash=newRefreshToken;
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
        if(string.IsNullOrWhiteSpace(refreshToken)) return false;

        var user=await _userRepository.GetByRefreshTokenHashAsync(refreshToken);

        if(user is null) return false;

        user.RefreshTokenHash=null;
        user.RefreshTokenExpiryTime=null;

        await _userRepository.SaveChangesAsync();
        
        return true;


    }
}