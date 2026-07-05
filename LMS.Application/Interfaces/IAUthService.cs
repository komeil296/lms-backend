using LMS.Application.DTOs.Auth;

namespace LMS.Application.Interfaces;
public interface IAUthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<(string? accessToken,string? refreshToken)> LoginAsync(LoginDto dto);
    Task<string?> RefreshTokenAsync(string refreshToken);
}