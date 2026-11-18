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

            // Сервіси для всіх сутностей
            var studentService = new CrudServiceAsync<StudentModel>(new Repository<StudentModel>(context));
            var courseService = new CrudServiceAsync<CourseModel>(new Repository<CourseModel>(context));
            var enrollmentService = new CrudServiceAsync<EnrollmentModel>(new Repository<EnrollmentModel>(context));
            var teacherService = new CrudServiceAsync<TeacherModel>(new Repository<TeacherModel>(context));
            var classroomService = new CrudServiceAsync<ClassroomModel>(new Repository<ClassroomModel>(context));

            // Запускаємо оновлений сидер
            await UniversitySeeder.SeedAsync(studentService, courseService, enrollmentService, teacherService, classroomService);

            // Вивід детального звіту по студентах
            Console.WriteLine("\n==================================================");
            Console.WriteLine("          ДЕТАЛЬНИЙ ЗВІТ ПО СТУДЕНТАХ (SQLite)     ");
            Console.WriteLine("==================================================");

            var students = await studentService.ReadAllAsync();
            var courses = await courseService.ReadAllAsync();
            var enrollments = await enrollmentService.ReadAllAsync();

            foreach (var student in students)
            {
                Console.WriteLine($"\n👤 Студент: {student.Name}");
                Console.WriteLine($"   ├── Група: {student.Group} | Вік: {student.Age} | Сер. бал: {student.AverageScore}");

                var studentEnrollments = enrollments.Where(e => e.StudentId == student.Id).ToList();
                if (studentEnrollments.Any())
                {
                    Console.WriteLine("   📚 Записані курси:");
                    foreach (var enrollment in studentEnrollments)
                    {
                        var course = courses.FirstOrDefault(c => c.Id == enrollment.CourseId);
                        if (course != null)
                        {
                            string gradeText = enrollment.Grade.HasValue ? $"{enrollment.Grade.Value} балів" : "Немає оцінки";
                            Console.WriteLine($"      • {course.Title} (Кредитів: {course.Credits}) ➔ Оцінка: {gradeText}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("   📚 Записані курси: Немає активних записів");
                }
                Console.WriteLine(new string('-', 50));
            }

            // Вивід детального звіту по викладачах, кабінетах і курсах (Зв'язки 1:1 та 1:N)
            Console.WriteLine("\n==================================================");
            Console.WriteLine("       ДЕТАЛЬНИЙ ЗВІТ ПО ВИКЛАДАЧАХ (SQLite)      ");
            Console.WriteLine("==================================================");

            var teachers = await teacherService.ReadAllAsync();
            var classrooms = await classroomService.ReadAllAsync();

            foreach (var teacher in teachers)
            {
                Console.WriteLine($"\n👨‍🏫 Викладач: {teacher.Name}");
                Console.WriteLine($"   ├── Вік: {teacher.Age} | Стаж: {teacher.AcademicExperienceYears} років | Зарплата: {teacher.Salary} грн");

                // Шукаємо кабінет (зв'язок 1:1)
                var classroom = classrooms.FirstOrDefault(c => c.TeacherId == teacher.Id);
                if (classroom != null)
                {
                    string projectorStatus = classroom.HasProjector ? "з проєктором" : "без проєктора";
                    Console.WriteLine($"   ├── Кабінет: №{classroom.RoomNumber} (Місткість: {classroom.Capacity} місць, {projectorStatus})");
                }
                else
                {
                    Console.WriteLine("   ├── Кабінет: Не закріплено");
                }

                // Шукаємо курси, які веде викладач (зв'язок 1:N)
                var teacherCourses = courses.Where(c => c.TeacherId == teacher.Id).ToList();
                if (teacherCourses.Any())
                {
                    Console.WriteLine("   └── Викладає курси:");
                    foreach (var course in teacherCourses)
                    {
                        Console.WriteLine($"      • {course.Title} (Кредитів: {course.Credits})");
                    }
                }
                else
                {
                    Console.WriteLine("   └── Викладає курси: Наразі немає призначених курсів");
                }

                Console.WriteLine(new string('-', 50));
            }
        }

        // ==========================================
        // 2. ДОДАТКОВЕ ЗАВДАННЯ: РОБОТА З MONGODB
        // ==========================================
        Console.WriteLine("\n=== Ініціалізація нереляційної бази (MongoDB) ===");
        try
        {
            MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
                new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)
            );

            var mongoStudentService = new CrudServiceAsync<StudentModel>(new MongoRepository<StudentModel>("mongodb://localhost:27017", "UniversityNoSqlDb", "Students"));
            var mongoCourseService = new CrudServiceAsync<CourseModel>(new MongoRepository<CourseModel>("mongodb://localhost:27017", "UniversityNoSqlDb", "Courses"));
            var mongoEnrollmentService = new CrudServiceAsync<EnrollmentModel>(new MongoRepository<EnrollmentModel>("mongodb://localhost:27017", "UniversityNoSqlDb", "Enrollments"));
            var mongoTeacherService = new CrudServiceAsync<TeacherModel>(new MongoRepository<TeacherModel>("mongodb://localhost:27017", "UniversityNoSqlDb", "Teachers"));
            var mongoClassroomService = new CrudServiceAsync<ClassroomModel>(new MongoRepository<ClassroomModel>("mongodb://localhost:27017", "UniversityNoSqlDb", "Classrooms"));

            await UniversitySeeder.SeedAsync(mongoStudentService, mongoCourseService, mongoEnrollmentService, mongoTeacherService, mongoClassroomService);

            Console.WriteLine("\n[MongoDB] Дані успішно засіяні для всіх сутностей.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MongoDB] Помилка (перевір Docker): {ex.Message}");
        }

        Console.WriteLine("\nРоботу програми завершено. Натисніть будь-яку клавішу для виходу.");
        Console.ReadKey();
    }
}