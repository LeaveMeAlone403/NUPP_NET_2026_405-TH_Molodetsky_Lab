using System;
using System.Text.Json.Serialization;

namespace University.Common;

// Делегат
public delegate void AgeChangedHandler(string personName, int oldAge, int newAge);

// Налаштування поліморфної десеріалізації для абстрактного класу
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Student), typeDiscriminator: "student")]
[JsonDerivedType(typeof(Teacher), typeDiscriminator: "teacher")]
public abstract class Person
{
  // Властивість
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;

  // Змінна
  private int _age;

  // Властивість
  public int Age
  {
    get => _age;
    set
    {
      if (_age != value)
      {
        int old = _age;
        _age = value;
        // Подія
        OnAgeChanged?.Invoke(Name, old, _age);
      }
    }
  }

  // Статичне поле
  public static int TotalPeopleCreated = 0;

  // Подія (ігнорується серіалізатором за замовчуванням)
  public event AgeChangedHandler? OnAgeChanged;

  // Статичний конструктор
  static Person()
  {
    TotalPeopleCreated = 0;
  }

  // Конструктор за замовчуванням (необхідний для JSON)
  public Person()
  {
    TotalPeopleCreated++;
  }

  // Конструктор з параметрами
  public Person(string name, int age) : this()
  {
    Name = name;
    Age = age;
  }

  // Метод
  public virtual string GetInfo()
  {
    return $"[{Id}] {Name}, вік: {Age}";
  }

  // Статичний метод
  public static int GetTotalCount()
  {
    return TotalPeopleCreated;
  }
}