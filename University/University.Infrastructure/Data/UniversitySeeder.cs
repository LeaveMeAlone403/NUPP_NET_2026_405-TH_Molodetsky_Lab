using University.Common;
using University.Infrastructure.Models;

namespace University.Infrastructure.Data
{
    public static class UniversitySeeder
    {
        public static async Task SeedAsync(
            ICrudServiceAsync<StudentModel> studentService,
            ICrudServiceAsync<CourseModel> courseService,
            ICrudServiceAsync<EnrollmentModel> enrollmentService,
            ICrudServiceAsync<TeacherModel> teacherService,
            ICrudServiceAsync<ClassroomModel> classroomService,
            bool clearBeforeSeed = true)
        {
            // 1. Повне очищення у правильному порядку (ззаду наперед за залежностями)
            if (clearBeforeSeed)
            {
                var enrollments = await enrollmentService.ReadAllAsync();
                foreach (var e in enrollments.ToList()) await enrollmentService.RemoveAsync(e);
                await enrollmentService.SaveAsync();

                var students = await studentService.ReadAllAsync();
                foreach (var s in students.ToList()) await studentService.RemoveAsync(s);
                await studentService.SaveAsync();

                var courses = await courseService.ReadAllAsync();
                foreach (var c in courses.ToList()) await courseService.RemoveAsync(c);
                await courseService.SaveAsync();

                var classrooms = await classroomService.ReadAllAsync();
                foreach (var cl in classrooms.ToList()) await classroomService.RemoveAsync(cl);
                await classroomService.SaveAsync();

                var teachers = await teacherService.ReadAllAsync();
                foreach (var t in teachers.ToList()) await teacherService.RemoveAsync(t);
                await teacherService.SaveAsync();

                Console.WriteLine("[UniversitySeeder] Бази даних успішно очищено від старих записів.");
            }

            // 2. Створення викладачів (без Subject)
            var teachersList = new List<TeacherModel>
            {
                new TeacherModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Олександр Запорожець",
                    Age = 45,
                    Salary = 32000m,
                    AcademicExperienceYears = 15
                },
                new TeacherModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Наталія Ковальчук",
                    Age = 38,
                    Salary = 28000m,
                    AcademicExperienceYears = 10
                },
                new TeacherModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Віктор Сидоренко",
                    Age = 52,
                    Salary = 38000m,
                    AcademicExperienceYears = 22
                }
            };

            foreach (var teacher in teachersList)
            {
                await teacherService.CreateAsync(teacher);
            }
            await teacherService.SaveAsync();

            // 3. Створення кабінетів (зв'язок 1:1 з викладачами)
            var classroomsList = new List<ClassroomModel>
            {
                new ClassroomModel
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = 301,
                    Capacity = 30,
                    HasProjector = true,
                    TeacherId = teachersList[0].Id
                },
                new ClassroomModel
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = 404,
                    Capacity = 25,
                    HasProjector = true,
                    TeacherId = teachersList[1].Id
                },
                new ClassroomModel
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = 512,
                    Capacity = 40,
                    HasProjector = false,
                    TeacherId = null
                }
            };

            foreach (var classroom in classroomsList)
            {
                await classroomService.CreateAsync(classroom);
            }
            await classroomService.SaveAsync();

            // 4. Створення курсів із прив'язкою до викладачів (TeacherId)
            var coursesList = new List<CourseModel>
            {
                new CourseModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Об'єктно-орієнтоване програмування",
                    Credits = 5,
                    TeacherId = teachersList[0].Id // Веде Олександр
                },
                new CourseModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Бази даних та SQL",
                    Credits = 4,
                    TeacherId = teachersList[1].Id // Веде Наталія
                },
                new CourseModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Алгоритми та структури даних",
                    Credits = 5,
                    TeacherId = teachersList[0].Id // Також веде Олександр
                },
                new CourseModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Архітектура програмного забезпечення",
                    Credits = 6,
                    TeacherId = teachersList[2].Id // Веде Віктор
                }
            };

            foreach (var course in coursesList)
            {
                await courseService.CreateAsync(course);
            }
            await courseService.SaveAsync();

            // 5. Створення студентів
            var studentsList = new List<StudentModel>
            {
                new StudentModel { Id = Guid.NewGuid(), Name = "Ярослав Молодецький", Age = 21, Group = "405-ТН", LastGrade = 95, AverageScore = 4.85 },
                new StudentModel { Id = Guid.NewGuid(), Name = "Олександр Петренко", Age = 20, Group = "405-ТН", LastGrade = 88, AverageScore = 4.20 },
                new StudentModel { Id = Guid.NewGuid(), Name = "Іван Коваленко", Age = 22, Group = "305-ТН", LastGrade = 92, AverageScore = 4.50 },
                new StudentModel { Id = Guid.NewGuid(), Name = "Марія Шевченко", Age = 20, Group = "305-ТН", LastGrade = 97, AverageScore = 4.90 },
                new StudentModel { Id = Guid.NewGuid(), Name = "Софія Мельник", Age = 21, Group = "501-ТН", LastGrade = 91, AverageScore = 4.45 }
            };

            foreach (var student in studentsList)
            {
                await studentService.CreateAsync(student);
            }
            await studentService.SaveAsync();

            // 6. Створення зв'язків (Enrollments) між студентами та курсами
            var enrollmentsList = new List<EnrollmentModel>
            {
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[0].Id, CourseId = coursesList[0].Id, Grade = 95 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[0].Id, CourseId = coursesList[1].Id, Grade = 90 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[1].Id, CourseId = coursesList[1].Id, Grade = 82 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[1].Id, CourseId = coursesList[2].Id, Grade = 85 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[2].Id, CourseId = coursesList[0].Id, Grade = 88 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[2].Id, CourseId = coursesList[3].Id, Grade = 92 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[3].Id, CourseId = coursesList[0].Id, Grade = 98 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[3].Id, CourseId = coursesList[1].Id, Grade = 95 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[3].Id, CourseId = coursesList[2].Id, Grade = 99 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[3].Id, CourseId = coursesList[3].Id, Grade = 97 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[4].Id, CourseId = coursesList[2].Id, Grade = 91 },
                new EnrollmentModel { Id = Guid.NewGuid(), StudentId = studentsList[4].Id, CourseId = coursesList[3].Id, Grade = 89 }
            };

            foreach (var enrollment in enrollmentsList)
            {
                await enrollmentService.CreateAsync(enrollment);
            }
            await enrollmentService.SaveAsync();

            Console.WriteLine($"[UniversitySeeder] Засіяно: {teachersList.Count} викладачів, {classroomsList.Count} кабінетів, {coursesList.Count} курсів, {studentsList.Count} студентів та {enrollmentsList.Count} енролментів.");
        }
    }
}