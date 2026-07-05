using System.Security.Cryptography;
using System.Text;
using LMS.Application.Interfaces;

namespace LMS.Infrastructure.Services;
public class PasswordService : IPasswordService
{
      public string HashPassword(string password)
    {
        using var sha=SHA256.Create();
        var bytes=Encoding.UTF8.GetBytes(password);
        var hash=sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
    public bool VerifyPassword(string password,string hashedPassword)
    {
        var newHash=HashPassword(password);
        return newHash==hashedPassword;
    }
}