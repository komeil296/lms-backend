using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface ITokenService{
    string CreateToken(User user);
}