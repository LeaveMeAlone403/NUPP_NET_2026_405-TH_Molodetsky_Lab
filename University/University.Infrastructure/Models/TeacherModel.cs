// Models/TeacherModel.cs
using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;

public class TeacherModel : PersonModel
{
  [Required, MaxLength(100)]
  public string Subject { get; set; } = string.Empty;

  public decimal Salary { get; set; }
  public int AcademicExperienceYears { get; set; }

  // 1:1 — викладач має один кабінет
  public ClassroomModel? Classroom { get; set; }
}