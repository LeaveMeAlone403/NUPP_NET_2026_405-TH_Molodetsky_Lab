// Repositories/Repository.cs
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
  private readonly UniversityContext _context;
  private readonly DbSet<T> _dbSet;

  public Repository(UniversityContext context)
  {
    _context = context;
    _dbSet = context.Set<T>();
  }

  public async Task<T?> GetByIdAsync(Guid id)
  {
    return await _dbSet.FindAsync(id);
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _dbSet.ToListAsync();
  }

  public async Task AddAsync(T entity)
  {
    await _dbSet.AddAsync(entity);
  }

  public Task Update(T entity)
  {
    _dbSet.Update(entity);
    return Task.CompletedTask;
  }

  public Task Delete(T entity)
  {
    _dbSet.Remove(entity);
    return Task.CompletedTask;
  }

  public async Task<int> SaveChangesAsync()
  {
    return await _context.SaveChangesAsync();
  }
}