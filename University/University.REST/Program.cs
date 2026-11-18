using Microsoft.EntityFrameworkCore;
using University.Common;
using University.Infrastructure.Data; // Переконайся, що простір імен твого контексту збігається
using University.Infrastructure.Models;
using University.Infrastructure.Repositories;
using University.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Додаємо контролери
builder.Services.AddControllers();

// 2. Swagger / OpenAPI для тестування
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. РЕЄСТРУЄМО КОНТЕКСТ БАЗИ ДАНИХ (Ось чого не вистачало!)
builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Реєструємо загальний репозиторій
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 5. Реєструємо сервіси для всіх 5 доменних моделей
builder.Services.AddScoped<ICrudServiceAsync<CourseModel>, CrudServiceAsync<CourseModel>>();
builder.Services.AddScoped<ICrudServiceAsync<EnrollmentModel>, CrudServiceAsync<EnrollmentModel>>();
builder.Services.AddScoped<ICrudServiceAsync<StudentModel>, CrudServiceAsync<StudentModel>>();
builder.Services.AddScoped<ICrudServiceAsync<TeacherModel>, CrudServiceAsync<TeacherModel>>();
builder.Services.AddScoped<ICrudServiceAsync<ClassroomModel>, CrudServiceAsync<ClassroomModel>>();

var app = builder.Build();

// 6. HTTP конвеєр
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();