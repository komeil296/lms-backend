using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByUSernameAsync(string username);
    Task AddAsync(User user);
    Task SaveChangesAsync();
    Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash);
}