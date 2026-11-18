using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;

public class CourseModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public int Credits { get; set; }

    // Зовнішній ключ на викладача (1 викладач -> багато курсів)
    public Guid? TeacherId { get; set; }
    public TeacherModel? Teacher { get; set; }

    // Колекція записів студентів на цей курс
    public ICollection<EnrollmentModel> Enrollments { get; set; } = new List<EnrollmentModel>();
}