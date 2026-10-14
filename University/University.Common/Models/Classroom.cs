namespace University.Common;

public class Classroom
{
  // Властивість
  public Guid Id { get; set; } = Guid.NewGuid();

  // Властивість
  public int RoomNumber { get; set; }

  // Властивість
  public int Capacity { get; set; }

  // Властивість
  public bool HasProjector { get; set; }

  // Конструктор
  public Classroom() { }

  // Конструктор з параметрами
  public Classroom(int roomNumber, int capacity, bool hasProjector)
  {
    RoomNumber = roomNumber;
    Capacity = capacity;
    HasProjector = hasProjector;
  }

  // Метод
  public string GetSummary()
  {
    string projectorStr = HasProjector ? "Є проектор" : "Без проектора";
    return $"Аудиторія №{RoomNumber}, Місткість: {Capacity} осіб ({projectorStr})";
  }
}