// Models/ClassroomModel.cs
using System.ComponentModel.DataAnnotations;
namespace University.Infrastructure.Models;

public class ClassroomModel
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  public int RoomNumber { get; set; }
  public int Capacity { get; set; }
  public bool HasProjector { get; set; }

  // 1:1 зворотний зв’язок
  public Guid? TeacherId { get; set; }
  public TeacherModel? Teacher { get; set; }
}