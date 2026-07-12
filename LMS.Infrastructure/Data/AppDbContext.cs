using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data;
public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users=>Set<User>();
    public DbSet<Course> Courses=>Set<Course>();
    public DbSet<Enrollment> Enrollments=>Set<Enrollment>();
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureUser(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureEnrollment(modelBuilder);
    }
    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        var user=modelBuilder.Entity<User>();
        user.ToTable("Users");
        user.HasKey(u=>u.Id);
        user.Property(u=>u.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
        user.Property(u=>u.Username).HasMaxLength(100).IsRequired();
        user.Property(u=>u.PasswordHash).HasMaxLength(500);
        user.Property(u=>u.RefreshTokenHash).HasMaxLength(64);
        user.HasIndex(u=>u.Username).IsUnique();
    }

    private static void ConfigureCourse(ModelBuilder modelBuilder)
    {
        var course=modelBuilder.Entity<Course>();
        course.ToTable("Courses");
        course.HasKey(c=>c.Id);
        course.Property(c=>c.Title).HasMaxLength(200).IsRequired();
        course.Property(c=>c.Description).HasMaxLength(2000).IsRequired();
        course.Property(c => c.Price).HasPrecision(18, 2);
        course.HasOne(c=>c.Teacher).WithMany(u=>u.Courses).HasForeignKey(c=>c.TeacherId).OnDelete(DeleteBehavior.Restrict);

    }

    private static void ConfigureEnrollment(ModelBuilder modelBuilder)
    {
        var enrollment=modelBuilder.Entity<Enrollment>();
        enrollment.ToTable("Enrollment");
        enrollment.Property(e=>e.EnrolledAt).IsRequired();
        enrollment.Property(e=>e.Status).HasConversion<int>().HasDefaultValue(EnrollmentStatus.Active).IsRequired();
        enrollment.HasKey(e=>e.Id);
        enrollment.HasOne(e=>e.Student).WithMany(u=>u.Enrollments).HasForeignKey(e=>e.StudentId).OnDelete(DeleteBehavior.Restrict);//dont delete all enrollment
        enrollment.HasOne(e=>e.Course).WithMany(c=>c.Enrollments).HasForeignKey(e=>e.CourseId).OnDelete(DeleteBehavior.Cascade);//delete all enrollments

        enrollment.HasIndex(e => new
        {
            e.StudentId,
            e.CourseId
        }).IsUnique();
    }
}