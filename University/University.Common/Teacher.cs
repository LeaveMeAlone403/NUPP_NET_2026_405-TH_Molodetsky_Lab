namespace University.Common;

public class Teacher : Person
{
  // Властивість
  public string Subject { get; set; } = string.Empty;
  public decimal Salary { get; set; }
  public int AcademicExperienceYears { get; set; }

  // Конструктор
  public Teacher() : base() { }

  // Конструктор з параметрами
  public Teacher(string name, int age, string subject, decimal salary, int experience)
      : base(name, age)
  {
    Subject = subject;
    Salary = salary;
    AcademicExperienceYears = experience;
  }

  // Метод
  public override string GetInfo()
  {
    return $"[Викладач] {Name}, Предмет: {Subject}, Стаж: {AcademicExperienceYears} років, ЗП: {Salary:C}";
  }
}