using LMS.Application.DTOs.Auth;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAUthService _authService;
    public AuthController(IAUthService authServcie)
    {
        _authService=authServcie;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result=await _authService.RegisterAsync(dto);
        if(!result) return BadRequest("user already Exist!");

        return Ok("User registered successfully!");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result=await _authService.LoginAsync(dto);
        if(result==null)return Unauthorized("Invalid Credentials!");
        return Ok(result);
    }
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut([FromBody] RefreshTokenRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
        {
            return BadRequest("Refresh token is required");
        }

        var result=await _authService.LogoutAsync(dto.RefreshToken);

        if (!result)
        {
            return Unauthorized("Invalid refresh token.");
        }
        return NoContent();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
        {
            return BadRequest("Refresh Token is required!");
        }

        var result=await _authService.RefreshTokenAsync(dto.RefreshToken);
        if(result is null)
        {
            return Unauthorized("Invalid or expired refresh token");
        }
        return Ok(result);
    }
}