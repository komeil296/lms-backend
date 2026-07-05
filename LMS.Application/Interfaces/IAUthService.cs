using LMS.Application.DTOs.Auth;

namespace LMS.Application.Interfaces;
public interface IAUthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}