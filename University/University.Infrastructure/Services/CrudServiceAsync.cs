// Services/CrudServiceAsync.cs  (можна покласти в Infrastructure або Common)
using University.Infrastructure.Repositories;
using University.Common;

namespace University.Infrastructure.Services;

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
  private readonly IRepository<T> _repository;

  public CrudServiceAsync(IRepository<T> repository)
  {
    _repository = repository;
  }

  public async Task<bool> CreateAsync(T element)
  {
    await _repository.AddAsync(element);
    return true;
  }

  public async Task<T?> ReadAsync(Guid id)
  {
    return await _repository.GetByIdAsync(id);
  }

  public async Task<IEnumerable<T>> ReadAllAsync()
  {
    return await _repository.GetAllAsync();
  }

  public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
  {
    var all = await _repository.GetAllAsync();
    return all.Skip((page - 1) * amount).Take(amount);
  }

  public async Task<bool> UpdateAsync(T element)
  {
    await _repository.Update(element);
    return true;
  }

  public async Task<bool> RemoveAsync(T element)
  {
    await _repository.Delete(element);
    return true;
  }

  public async Task<bool> SaveAsync()
  {
    var result = await _repository.SaveChangesAsync();
    return result > 0;
  }

  public IEnumerator<T> GetEnumerator()
  {
    // Можна повернути порожній, або зробити синхронно через репозиторій
    return _repository.GetAllAsync().GetAwaiter().GetResult().GetEnumerator();
  }

  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}