namespace LMS.Domain.Entities;
public class Lesson
{
    public Guid Id{get;set;}=Guid.NewGuid();
    public Guid CourseId{get;set;}
    public Course Course{get;set;}=null!;
    public string Title{get;set;}=string.Empty;
    public string Content{get;set;}=string.Empty;
    public string? VideoUrl {get;set;}
    public int OrderIndex{get;set;}
    public bool IsPublished{get;set;}
    public DateTime CreatedAt{get;set;}=DateTime.UtcNow;

}