using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Infrastructure.Services;
public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher=new();

    public string HashPassword(User user,string passsword)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(passsword);
        return _passwordHasher.HashPassword(user,passsword);
    }

    public  bool VerifyPassword(User user,string password,string hashedPassword)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);
        
        var result=_passwordHasher.VerifyHashedPassword(user,hashedPassword,password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}