using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Common;
using University.Infrastructure.Models;

namespace University.Infrastructure.Data
{
  public static class StudentSeeder
  {
    public static async Task SeedAsync(ICrudServiceAsync<StudentModel> studentService, bool clearBeforeSeed = true)
    {
      // 1. Повне очищення бази перед заповненням
      if (clearBeforeSeed)
      {
        var existingStudents = await studentService.ReadAllAsync();
        foreach (var student in existingStudents.ToList())
        {
          await studentService.RemoveAsync(student);
        }
        await studentService.SaveAsync();
        Console.WriteLine("[StudentSeeder] Базу даних успішно очищено.");
      }

      // 2. Список студентів без хардкодних ID
      var predefinedStudents = new List<StudentModel>
      {
          new StudentModel
          {
              Name = "Ярослав Молодецький",
              Age = 21,
              Group = "405-ТН",
              LastGrade = 95,
              AverageScore = 4.85
          },
          new StudentModel
          {
              Name = "Олександр Петренко",
              Age = 20,
              Group = "405-ТН",
              LastGrade = 88,
              AverageScore = 4.20
          },
          new StudentModel
          {
              Name = "Іван Коваленко",
              Age = 22,
              Group = "305-ТН",
              LastGrade = 92,
              AverageScore = 4.50
          },
          new StudentModel
          {
              Name = "Марія Шевченко",
              Age = 20,
              Group = "305-ТН",
              LastGrade = 97,
              AverageScore = 4.90
          },
          new StudentModel
          {
              Name = "Софія Мельник",
              Age = 21,
              Group = "501-ТН",
              LastGrade = 91,
              AverageScore = 4.45
          }
      };

      // 3. Генерація унікальних ID та додавання в базу
      foreach (var student in predefinedStudents)
      {
        if (student.Id == Guid.Empty)
        {
          student.Id = Guid.NewGuid();
        }
        await studentService.CreateAsync(student);
      }
      await studentService.SaveAsync();

      Console.WriteLine($"[StudentSeeder] Успішно додано {predefinedStudents.Count} студентів.");
    }
  }
}