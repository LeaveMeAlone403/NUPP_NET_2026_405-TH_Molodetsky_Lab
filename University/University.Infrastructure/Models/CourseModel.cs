// Models/CourseModel.cs  (для many-to-many)
using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;
public class CourseModel
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  [Required, MaxLength(100)]
  public string Title { get; set; } = string.Empty;

  public int Credits { get; set; }

  public ICollection<EnrollmentModel> Enrollments { get; set; } = new List<EnrollmentModel>();
}