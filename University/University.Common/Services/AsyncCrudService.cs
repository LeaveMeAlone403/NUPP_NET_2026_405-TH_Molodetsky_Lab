using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace University.Common;

public class AsyncCrudService<T> : ICrudServiceAsync<T> where T : class
{
  // Потокобезпечна колекція для забезпечення thread-safe операцій
  private readonly ConcurrentDictionary<Guid, T> _storage = new();
  private readonly string _filePath;

  // Примітив синхронізації SemaphoreSlim для безпечного доступу до файлу з різних потоків
  private readonly SemaphoreSlim _fileSemaphore = new(1, 1);

  public AsyncCrudService(string filePath)
  {
    _filePath = filePath;
  }

  public async Task<bool> CreateAsync(T element)
  {
    if (element == null) return false;
    var prop = typeof(T).GetProperty("Id");
    if (prop == null) return false;

    var id = (Guid)prop.GetValue(element)!;
    bool added = _storage.TryAdd(id, element);
    return await Task.FromResult(added);
  }

  public async Task<T?> ReadAsync(Guid id)
  {
    _storage.TryGetValue(id, out var element);
    return await Task.FromResult(element);
  }

  public async Task<IEnumerable<T>> ReadAllAsync()
  {
    return await Task.FromResult(_storage.Values.ToList());
  }

  // Реалізація пагінації за допомогою LINQ (Skip / Take)
  public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
  {
    if (page < 1) page = 1;
    if (amount < 1) amount = 10;

    var result = _storage.Values
        .Skip((page - 1) * amount)
        .Take(amount)
        .ToList();

    return await Task.FromResult(result);
  }

  public async Task<bool> UpdateAsync(T element)
  {
    if (element == null) return false;
    var prop = typeof(T).GetProperty("Id");
    if (prop == null) return false;

    var id = (Guid)prop.GetValue(element)!;
    if (_storage.ContainsKey(id))
    {
      _storage[id] = element;
      return await Task.FromResult(true);
    }
    return await Task.FromResult(false);
  }

  public async Task<bool> RemoveAsync(T element)
  {
    if (element == null) return false;
    var prop = typeof(T).GetProperty("Id");
    if (prop == null) return false;

    var id = (Guid)prop.GetValue(element)!;
    return await Task.FromResult(_storage.TryRemove(id, out _));
  }

  // Асинхронне збереження колекції у файл із використанням SemaphoreSlim
  public async Task<bool> SaveAsync()
  {
    await _fileSemaphore.WaitAsync();
    try
    {
      var options = new JsonSerializerOptions { WriteIndented = true };
      string json = JsonSerializer.Serialize(_storage.Values.ToList(), options);
      await File.WriteAllTextAsync(_filePath, json);
      return true;
    }
    catch
    {
      return false;
    }
    finally
    {
      _fileSemaphore.Release();
    }
  }

  public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();
  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}