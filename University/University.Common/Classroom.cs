namespace University.Common;

public class Classroom
{
  // Властивість
  public Guid Id { get; set; } = Guid.NewGuid();

  public int RoomNumber { get; set; }
  public int Capacity { get; set; }
  public bool HasProjector { get; set; }

  public Classroom() { }

  public Classroom(int roomNumber, int capacity, bool hasProjector)
  {
    RoomNumber = roomNumber;
    Capacity = capacity;
    HasProjector = hasProjector;
  }

  public string GetSummary()
  {
    string projectorStr = HasProjector ? "Є проектор" : "Без проектора";
    return $"Аудиторія №{RoomNumber}, Місткість: {Capacity} осіб ({projectorStr})";
  }
}