using System;
using University.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №1: University ===\n");

// 1. Демонстрація статичного методу і лічильника
Console.WriteLine($"Створено об'єктів Person до запуску: {Person.GetTotalCount()}");

// 2. Створення об'єктів
var student1 = new Student("Ярослав", 19, "405-ТН", 85, 91.5);
var student2 = new Student("Олександр", 20, "405-ТН", 78, 82.0);
var teacher = new Teacher("Іван Петрович", 45, "C# та .NET", 25000m, 15);

// Підписка на подію
student1.OnAgeChanged += (name, oldAge, newAge) =>
{
  Console.WriteLine($"[ПОДІЯ] Вік {name} змінився: з {oldAge} до {newAge} років");
};

Console.WriteLine($"Створено об'єктів Person після запуску: {Person.GetTotalCount()}\n");

// 3. Ініціалізація CRUD Сервісу
var personService = new InMemoryCrudService<Person>();

Console.WriteLine("--- 1. Додавання елементів (Create) ---");
personService.Create(student1);
personService.Create(student2);
personService.Create(teacher);

foreach (var item in personService.ReadAll())
{
  Console.WriteLine(item.GetInfo());
}

Console.WriteLine("\n--- 2. Демонстрація події та методу розширення (Update + Extension) ---");
// Зміна віку (триггерне подію OnAgeChanged)
student1.Age = 20;

// Метод розширення для бонусної оцінки
student1.AddBonusPoints(10);
personService.Update(student1);

Console.WriteLine($"Оновлена інформація про студента: {student1.GetInfo()}");

Console.WriteLine("\n--- 3. Читання за Id (Read) ---");
var foundPerson = personService.Read(teacher.Id);
Console.WriteLine($"Знайдено: {foundPerson?.GetInfo()}");

Console.WriteLine("\n--- 4. Видалення (Remove) ---");
personService.Remove(student2);
Console.WriteLine("Після видалення другого студента:");
foreach (var item in personService.ReadAll())
{
  Console.WriteLine(item.GetInfo());
}

Console.WriteLine("\n--- 5. Додаткове завдання (Save & Load JSON) ---");
string filePath = "people.json";
personService.Save(filePath);
Console.WriteLine($"Дані успішно збережено у файл: {Path.GetFullPath(filePath)}");

var newService = new InMemoryCrudService<Person>();
newService.Load(filePath);
Console.WriteLine("Дані успішно завантажено в новий сервіс з файлу:");
foreach (var item in newService.ReadAll())
{
  Console.WriteLine(item.GetInfo());
}