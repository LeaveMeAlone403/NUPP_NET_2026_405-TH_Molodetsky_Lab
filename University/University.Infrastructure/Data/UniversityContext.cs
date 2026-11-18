// Data/UniversityContext.cs
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Models;

namespace University.Infrastructure.Data;

public class UniversityContext : DbContext
{
    // Таблиці бази даних
    public DbSet<PersonModel> Persons { get; set; }
    public DbSet<StudentModel> Students { get; set; }
    public DbSet<TeacherModel> Teachers { get; set; }
    public DbSet<ClassroomModel> Classrooms { get; set; }
    public DbSet<CourseModel> Courses { get; set; }
    public DbSet<EnrollmentModel> Enrollments { get; set; }
    // Конструктор для DI (Dependency Injection), коли налаштування передаються ззовні
    public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }
    // Конструктор без параметрів (для міграцій або ручного створення через new UniversityContext())
    public UniversityContext() { }
    // Аварійне підключення: спрацьовує, якщо конфігурація ззовні не була передана
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=university.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Спадкування TPT (Table-per-Type)
        modelBuilder.Entity<PersonModel>().ToTable("Persons");
        modelBuilder.Entity<StudentModel>().ToTable("Students");
        modelBuilder.Entity<TeacherModel>().ToTable("Teachers");

        // 2. Зв'язок 1:1 (Викладач ↔ Кабінет)
        modelBuilder.Entity<TeacherModel>()
            .HasOne(t => t.Classroom)
            .WithOne(c => c.Teacher)
            .HasForeignKey<ClassroomModel>(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // 3. Зв'язок 1:N (Викладач → Курси)
        modelBuilder.Entity<CourseModel>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // 4. Зв'язок 1:N (Студент → Записи)
        modelBuilder.Entity<EnrollmentModel>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // 5. Зв'язок Багато-до-Багато через Enrollment (Курс ↔ Записи)
        modelBuilder.Entity<EnrollmentModel>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // 6. Автоматичне підтягування зв'язків (AutoInclude)
        modelBuilder.Entity<StudentModel>().Navigation(s => s.Enrollments).AutoInclude();
        modelBuilder.Entity<CourseModel>().Navigation(c => c.Enrollments).AutoInclude();
        modelBuilder.Entity<CourseModel>().Navigation(c => c.Teacher).AutoInclude();
        modelBuilder.Entity<TeacherModel>().Navigation(t => t.Classroom).AutoInclude();
        modelBuilder.Entity<TeacherModel>().Navigation(t => t.Courses).AutoInclude();

        // 7. Обмеження та індекси
        modelBuilder.Entity<ClassroomModel>().HasIndex(c => c.RoomNumber).IsUnique();
        modelBuilder.Entity<CourseModel>().Property(c => c.Title).HasMaxLength(100);
    }
}