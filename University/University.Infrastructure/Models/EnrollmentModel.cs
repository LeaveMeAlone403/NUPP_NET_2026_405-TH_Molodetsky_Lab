// Models/EnrollmentModel.cs  (join-таблиця many-to-many)
using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;

public class EnrollmentModel
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  public Guid StudentId { get; set; }
  public StudentModel Student { get; set; } = null!;

  public Guid CourseId { get; set; }
  public CourseModel Course { get; set; } = null!;

  public int? Grade { get; set; }
}