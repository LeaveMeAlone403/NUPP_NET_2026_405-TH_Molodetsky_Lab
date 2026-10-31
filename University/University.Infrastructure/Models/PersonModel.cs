// Models/PersonModel.cs
using System.ComponentModel.DataAnnotations;

namespace University.Infrastructure.Models;

public abstract class PersonModel
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  [Required, MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  public int Age { get; set; }

  // Навігаційні властивості
  public StudentModel? Student { get; set; }
  public TeacherModel? Teacher { get; set; }
}
