namespace University.Common;

public class Student : Person
{
  // Властивість
  public string Group { get; set; } = string.Empty;
  public int LastGrade { get; set; }
  public double AverageScore { get; set; }
  public Student() : base() { }

  // Конструктор з параметрами
  public Student(string name, int age, string group, int lastGrade, double averageScore)
      : base(name, age)
  {
    Group = group;
    LastGrade = lastGrade;
    AverageScore = averageScore;
  }

  // Метод
  public override string GetInfo()
  {
    return $"[Студент] {Name}, Група: {Group}, Оцінка: {LastGrade}, Середній бал: {AverageScore:F1}, Вік: {Age}";
  }
}