using LMS.Domain.Enums;

namespace LMS.Domain.Entities;
public class Enrollment
{
    public Guid Id{get;set;}=Guid.NewGuid();
    public Guid StudentId{get;set;}
    public User Student{get;set;}=null!;
    public Guid CourseId{get;set;}
    public Course Course{get;set;}=null!;
    public DateTime EnrolledAt{get;set;}=DateTime.UtcNow;
    public EnrollmentStatus Status{get;set;}=EnrollmentStatus.Active;
}