// Data/UniversityContext.cs
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Models;

namespace University.Infrastructure.Data;

public class UniversityContext : DbContext
{
  public DbSet<PersonModel> Persons { get; set; }
  public DbSet<StudentModel> Students { get; set; }
  public DbSet<TeacherModel> Teachers { get; set; }
  public DbSet<ClassroomModel> Classrooms { get; set; }
  public DbSet<CourseModel> Courses { get; set; }
  public DbSet<EnrollmentModel> Enrollments { get; set; }

  public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

  // Для SQLite / консолі
  public UniversityContext() { }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      // SQLite (зручно для здачі)
      optionsBuilder.UseSqlite("Data Source=university.db");

      // Або PostgreSQL / SQL Server:
      // optionsBuilder.UseNpgsql("Host=localhost;Database=university;Username=...;Password=...");
      // optionsBuilder.UseSqlServer("Server=...;Database=University;...");
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // === Table-per-Type (TPT) ===
    modelBuilder.Entity<PersonModel>().ToTable("Persons");
    modelBuilder.Entity<StudentModel>().ToTable("Students");
    modelBuilder.Entity<TeacherModel>().ToTable("Teachers");

    // === 1:1  Teacher ↔ Classroom ===
    modelBuilder.Entity<TeacherModel>()
        .HasOne(t => t.Classroom)
        .WithOne(c => c.Teacher)
        .HasForeignKey<ClassroomModel>(c => c.TeacherId)
        .OnDelete(DeleteBehavior.SetNull);

    // === 1:N  Student → Enrollments ===
    modelBuilder.Entity<EnrollmentModel>()
        .HasOne(e => e.Student)
        .WithMany(s => s.Enrollments)
        .HasForeignKey(e => e.StudentId)
        .OnDelete(DeleteBehavior.Cascade);

    // === Many-to-Many через Enrollment ===
    modelBuilder.Entity<EnrollmentModel>()
        .HasOne(e => e.Course)
        .WithMany(c => c.Enrollments)
        .HasForeignKey(e => e.CourseId)
        .OnDelete(DeleteBehavior.Cascade);

    // Індекси / обмеження
    modelBuilder.Entity<ClassroomModel>()
        .HasIndex(c => c.RoomNumber)
        .IsUnique();

    modelBuilder.Entity<CourseModel>()
        .Property(c => c.Title)
        .HasMaxLength(100);
  }
}