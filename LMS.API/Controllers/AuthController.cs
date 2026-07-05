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
    public async Task<IActionResult> LogOut(string refreshToken)
    {
        var result=await _authService.LogoutAsync(refreshToken);
        if(!result) return BadRequest("Invalid Token!");
        return Ok("Logged out successfully!");
    }
}