namespace University.Common;

public class Student : Person
{
  // Властивість
  public string Group { get; set; } = string.Empty;

  // Властивість
  public int LastGrade { get; set; }

  // Властивість
  public double AverageScore { get; set; }

  // Конструктор
  public Student() : base() { }

  // Конструктор з параметрами
  public Student(string name, int age, string group, int lastGrade, double averageScore)
      : base(name, age)
  {
    Group = group;
    LastGrade = lastGrade;
    AverageScore = averageScore;
  }

  public static Student CreateNew()
  {
    var random = new Random();
    string[] names = { "Олександр", "Марія", "Іван", "Софія", "Андрій", "Катерина", "Дмитро", "Анастасія" };
    string[] groups = { "305-ТН", "405-ТН", "301-ТН", "402-ТН" };

    string randomName = names[random.Next(names.Length)];
    int randomAge = random.Next(17, 25);
    string randomGroup = groups[random.Next(groups.Length)];
    int randomGrade = random.Next(60, 100);
    double randomScore = random.NextDouble() * 40 + 60; // від 60 до 100

    return new Student(randomName, randomAge, randomGroup, randomGrade, randomScore);
  }

  // Метод
  public override string GetInfo()
  {
    return $"[Студент] {Name}, Група: {Group}, Оцінка: {LastGrade}, Середній бал: {AverageScore:F1}, Вік: {Age}";
  }
}