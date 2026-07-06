namespace LMS.Domain.Entities;
public class Course
{
    public Guid Id{get;set;}
    public string Title{get;set;}=string.Empty;
    public string Description{get;set;}=string.Empty;
    public decimal Price{get;set;}
    public DateTime CreatedAt{get;set;}=DateTime.UtcNow;
}