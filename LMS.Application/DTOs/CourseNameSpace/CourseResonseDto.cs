namespace LMS.Application.DTOs.CourseNameSpace;
public class CourseResonseDto
{
    public Guid ID{get;set;}
    public string Title{get;set;}=string.Empty;
    public string Description{get;set;}=string.Empty;
    public decimal Price{get;set;}
    public DateTime CreatedAt{get;set;}
    public string TeacherName{get;set;}=string.Empty;
    
}