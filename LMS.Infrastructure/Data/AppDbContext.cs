using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data;
public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users=>Set<User>();
    public DbSet<Course> Courses{get;set;}
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().Property(u=>u.Role).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<User>().Property(u=>u.Username).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u=>u.PasswordHash).HasMaxLength(500);
        modelBuilder.Entity<User>().Property(u=>u.RefreshTokenHash).HasMaxLength(64);
        modelBuilder.Entity<User>().HasIndex(u=>u.Username).IsUnique();

      modelBuilder.Entity<Course>().Property(c=>c.Title).HasMaxLength(200);
      modelBuilder.Entity<Course>().Property(c=>c.Description).HasMaxLength(2000);

        modelBuilder.Entity<Course>().HasOne(c=>c.Teacher).WithMany(u=>u.Courses).HasForeignKey(c=>c.TeacherId).OnDelete(DeleteBehavior.Restrict);

       
        modelBuilder.Entity<Enrollment>().HasKey(e=>e.Id);
        modelBuilder.Entity<Enrollment>().HasOne(e=>e.Student).WithMany(u=>u.Enrollments).HasForeignKey(e=>e.StudentId).OnDelete(DeleteBehavior.Restrict);//dont delete all enrollment
        modelBuilder.Entity<Enrollment>().HasOne(e=>e.Course).WithMany(c=>c.Enrollments).HasForeignKey(e=>e.CourseId).OnDelete(DeleteBehavior.Cascade);//delete all enrollments

        modelBuilder.Entity<Enrollment>().HasIndex(e => new
        {
            e.StudentId,
            e.CourseId
        }).IsUnique();

    }
}