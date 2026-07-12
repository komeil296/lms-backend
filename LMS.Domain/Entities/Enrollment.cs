using LMS.Domain.Enums;

namespace LMS.Domain.Entities;
public class Enrollment
{
    public Guid Id{get;set;}
    public Guid StudentId{get;set;}
    public User Student{get;set;}
    public Guid CourseId{get;set;}
    public Course Course{get;set;}
    public DateTime EnrollAt{get;set;}
    public EnrollmentStatus Status{get;set;}
}