using LMS.Domain.Enums;

namespace LMS.Application.DTOs.Enrollment;
public sealed class EnrollmentResponseDto
{
    public Guid Id{get;set;}
    public Guid CourseId{get;set;}
    public string CourseTitle{get;set;}=string.Empty;
    public string TeacherName{get;set;}=string.Empty;
    public decimal Price{get;set;}
    public DateTime EnrolledAt{get;set;}
    public EnrollmentStatus Status{get;set;}
}