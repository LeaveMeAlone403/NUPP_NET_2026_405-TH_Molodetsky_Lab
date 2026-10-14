using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using University.Common;

class Program
{
  static async Task Main(string[] args)
  {
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    string filePath = "students_async_data.json";
    var service = new AsyncCrudService<Student>(filePath);

    int totalItems = 2000; 
    Console.WriteLine($"=== ЛАБОРАТОРНА РОБОТА №2 ===");
    Console.WriteLine($"[INFO] Паралельна генерація та додавання {totalItems} студентів через Parallel.For...\n");

    var stopwatch = Stopwatch.StartNew();

    // 1. Паралельне створення об'єктів за допомогою Parallel.For та потокобезпечного сервісу
    Parallel.For(0, totalItems, i =>
    {
      var student = Student.CreateNew();
      service.CreateAsync(student).GetAwaiter().GetResult();
    });

    stopwatch.Stop();
    Console.WriteLine($"[INFO] Успішно згенеровано та додано за {stopwatch.ElapsedMilliseconds} мс.");

    // 2. Отримання всіх елементів для аналізу через LINQ
    var allStudents = (await service.ReadAllAsync()).ToList();

    if (allStudents.Any())
    {
      Console.WriteLine("\n=== АНАЛІЗ ДАНИХ ЗА ДОПОМОГОЮ LINQ ===");

      // Знаходження мінімального, максимального та середнього для числових властивостей
      int minAge = allStudents.Min(s => s.Age);
      int maxAge = allStudents.Max(s => s.Age);
      double avgAge = allStudents.Average(s => s.Age);

      int minGrade = allStudents.Min(s => s.LastGrade);
      int maxGrade = allStudents.Max(s => s.LastGrade);
      double avgGrade = allStudents.Average(s => s.LastGrade);

      double avgScore = allStudents.Average(s => s.AverageScore);

      Console.WriteLine($"Вік:      Мін = {minAge}, Макс = {maxAge}, Середній = {avgAge:F1} років");
      Console.WriteLine($"Оцінки:   Мін = {minGrade}, Макс = {maxGrade}, Середня = {avgGrade:F1}");
      Console.WriteLine($"Сер. бал: Загальний середній бал по потоку = {avgScore:F2}");
    }

    // 3. Демонстрація пагінації
    Console.WriteLine("\n=== ДЕМОНСТРАЦІЯ ПАГІНАЦІЇ (Сторінка 1, 5 елементів) ===");
    var page1 = await service.ReadAllAsync(1, 5);
    foreach (var s in page1)
    {
      Console.WriteLine(s.GetInfo());
    }

    // 4. Асинхронне збереження у файл
    Console.WriteLine("\n[INFO] Збереження колекції у файл асинхронічно...");
    bool saved = await service.SaveAsync();
    Console.WriteLine(saved ? "[INFO] Файл успішно збережено!" : "[ERROR] Помилка збереження файлу.");

    // 5. Демонстрація примітивів синхронізації (lock, AutoResetEvent, Semaphore)
    Console.WriteLine("\n=== ДЕМОНСТРАЦІЯ ПРИМІТІВІВ СИНХРОНІЗАЦІЇ ===");

    // Приклад lock
    object syncLock = new object();
    Parallel.Invoke(
        () => { lock (syncLock) { Console.WriteLine("[Lock] Потік 1 увійшов у критичну секцію."); } },
        () => { lock (syncLock) { Console.WriteLine("[Lock] Потік 2 увійшов у критичну секцію."); } }
    );

    // Приклад AutoResetEvent
    AutoResetEvent autoEvent = new AutoResetEvent(false);
    Task.Run(() =>
    {
      Thread.Sleep(100);
      Console.WriteLine("[AutoResetEvent] Сигнал з фонового потоку надіслано.");
      autoEvent.Set();
    });
    autoEvent.WaitOne(); // Очікування сигналу
    Console.WriteLine("[AutoResetEvent] Головний потік продовжив виконання після сигналу.");
  }
}