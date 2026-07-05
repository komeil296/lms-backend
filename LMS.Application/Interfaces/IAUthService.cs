using LMS.Application.DTOs;
using LMS.Application.DTOs.Auth;

namespace LMS.Application.Interfaces;
public interface IAUthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(string refreshToken);
}