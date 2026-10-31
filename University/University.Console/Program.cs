using Microsoft.EntityFrameworkCore;
using University.Common;
using University.Infrastructure.Data;
using University.Infrastructure.Models;
using University.Infrastructure.Repositories;
using University.Infrastructure.Services;
using University.Nosql;

class Program
{
  static async Task Main(string[] args)
  {
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    Console.WriteLine("=== Ініціалізація університетської системи (.NET 8) ===\n");

    // ==========================================
    // 1. РОБОТА З РЕЛЯЦІЙНОЮ БАЗОЮ (SQLite / EF Core)
    // ==========================================
    var optionsBuilder = new DbContextOptionsBuilder<UniversityContext>();
    optionsBuilder.UseSqlite("Data Source=university.db");

    using (var context = new UniversityContext(optionsBuilder.Options))
    {
      await context.Database.MigrateAsync();
      Console.WriteLine("[SQLite] База даних успішно створена та оновлена міграціями.");

      IRepository<StudentModel> sqliteRepository = new Repository<StudentModel>(context);
      ICrudServiceAsync<StudentModel> sqliteService = new CrudServiceAsync<StudentModel>(sqliteRepository);

      // Наповнюємо базу даних студентами за допомогою StudentSeeder (без дублікатів)
      await StudentSeeder.SeedAsync(sqliteService);

      Console.WriteLine("\n--- Повний список студентів у SQLite ---");
      var students = await sqliteService.ReadAllAsync();
      foreach (var student in students)
      {
        Console.WriteLine($"ID: {student.Id} | Група: {student.Group} | Ім'я: {student.Name} | Бал: {student.AverageScore}");
      }
    }

    // ==========================================
    // 2. ДОДАТКОВЕ ЗАВДАННЯ: РОБОТА З MONGODB
    // ==========================================
    Console.WriteLine("\n=== Ініціалізація нереляційної бази (MongoDB) ===");
    try
    {
      // Виправляємо серіалізацію Guid для MongoDB драйвера 3.x
      MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
          new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)
      );

      IRepository<StudentModel> mongoRepository = new MongoRepository<StudentModel>(
          "mongodb://localhost:27017",
          "UniversityNoSqlDb",
          "Students"
      );

      ICrudServiceAsync<StudentModel> mongoService = new CrudServiceAsync<StudentModel>(mongoRepository);

      // Використовуємо той самий StudentSeeder для MongoDB
      await StudentSeeder.SeedAsync(mongoService);

      Console.WriteLine("\n--- Повний список студентів з MongoDB ---");
      var mongoStudents = await mongoService.ReadAllAsync();
      foreach (var student in mongoStudents)
      {
        Console.WriteLine($"-> ID: {student.Id} | Група: {student.Group} | Ім'я: {student.Name} | Бал: {student.AverageScore}");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"[MongoDB] Помилка (перевір, чи запущена служба MongoDB у Docker): {ex.Message}");
    }

    Console.WriteLine("\nРоботу програми завершено. Натисніть будь-яку клавішу для виходу.");
    Console.ReadKey();
  }
}