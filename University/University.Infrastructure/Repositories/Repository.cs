using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly UniversityContext _context;
    private readonly DbSet<T> _dbSet;

    // Ініціалізуємо контекст бази даних та отримуємо DbSet для конкретної сутності
    public Repository(UniversityContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Отримання сутності за унікальним ідентифікатором (GUID)
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Отримання списку всіх записів з таблиці
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Додавання нового запису в контекст
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // Оновлення існуючого запису
    public Task Update(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    // Видалення запису
    public Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    // Збереження всіх змінених даних у базу (Commit)
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}