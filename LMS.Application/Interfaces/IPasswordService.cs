using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface IPasswordService
{
    string HashPassword(User user,string passsword);
    bool VerifyPassword(User user,string password,string hashedPassword);
}