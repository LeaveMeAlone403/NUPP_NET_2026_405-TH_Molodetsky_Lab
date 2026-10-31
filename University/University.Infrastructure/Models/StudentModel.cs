// Models/StudentModel.cs
using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;

public class StudentModel : PersonModel
{
  [Required, MaxLength(20)]
  public string Group { get; set; } = string.Empty;

  public int LastGrade { get; set; }
  public double AverageScore { get; set; }

  // 1:N — студент має багато оцінок / записів
  public ICollection<EnrollmentModel> Enrollments { get; set; } = new List<EnrollmentModel>();
}