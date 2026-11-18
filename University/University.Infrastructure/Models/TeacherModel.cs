using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;

public class TeacherModel : PersonModel
{
    public decimal Salary { get; set; }
    public int AcademicExperienceYears { get; set; }

    // 1:1 — викладач має один кабінет
    public ClassroomModel? Classroom { get; set; }

    // 1:N — викладач веде багато курсів
    public ICollection<CourseModel> Courses { get; set; } = new List<CourseModel>();
}