namespace LMS.Domain.Entities;
public class User
{
    public Guid Id{get;set;}
    public string Username{get;set;}=string.Empty;
    public string PasswordHash{get;set;}=string.Empty;

    public DateTime CraetedAt{get;set;}=DateTime.UtcNow;
    public string Role{get;set;}="Student";
    public string? RefreshToken{get;set;}
    public DateTime? RefreshTokenExpiryTime{get;set;}
}