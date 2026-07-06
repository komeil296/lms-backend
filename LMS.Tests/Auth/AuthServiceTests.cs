namespace LMS.Tests.Auth;

using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Application.DTOs.Auth;
using Moq;
using Xunit;
using FluentAssertions;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock=new();
    private readonly Mock<IPasswordService> _passwordMock=new();
    private readonly Mock<AutoMapper.IMapper> _mapperMock=new();
    private readonly Mock<ILogger<AuthService>> _loggerMock=new();
    private readonly Mock<ITokenService> _tokenMock=new();

    private AuthService CreateService()
    {
        return new AuthService(_userRepoMock.Object,_passwordMock.Object,_mapperMock.Object,_loggerMock.Object,_tokenMock.Object);
    }
    [Fact]
    public async Task Register_ShouldReturnTrue_WhenUserDoesNotExist()
    {
        _userRepoMock.Setup(x=>x.GetByUSernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _passwordMock.Setup(x=>x.HashPassword(It.IsAny<string>())).Returns("hashed_password");
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<RegisterDto>())).Returns(new User());

        var service=CreateService();
        var result=await service.RegisterAsync(new RegisterDto
        {
            Username="test",
            Password="123"
        });
        Assert.True(result);
    }
   [Fact]
    public async Task Login_ShouldReturnNull_WhenPasswordIncorrect()
    {
        User user = new()
        {
            Username="username",
            PasswordHash="HashedPassword"
        };
        LoginDto dto = new()
        {
            Username="username",
            Password="dto_passwrod"
        };
        _userRepoMock.Setup(x=>x.GetByUSernameAsync(dto.Username)).ReturnsAsync(user);
        _passwordMock.Setup(x=>x.VerifyPassword(dto.Password,user.PasswordHash)).Returns(false);
        var service=CreateService();
        var result = await service.LoginAsync(dto);

        result.Should().BeNull();
    }

}