using System.Text.Json;

namespace University.Common;

public class InMemoryCrudService<T> : ICrudService<T> where T : class
{
  private readonly List<T> _items = [];

  public void Create(T element) => _items.Add(element);

  public IEnumerable<T> ReadAll() => _items;

  public T? Read(Guid id)
  {
    var prop = typeof(T).GetProperty("Id");
    if (prop == null) return null;

    return _items.FirstOrDefault(x => (Guid?)prop.GetValue(x) == id);
  }

  public void Update(T element)
  {
    var prop = typeof(T).GetProperty("Id");
    if (prop == null) return;

    var id = (Guid?)prop.GetValue(element);
    if (id == null) return;

    var existing = Read(id.Value);
    if (existing != null)
    {
      _items.Remove(existing);
      _items.Add(element);
    }
  }

  public void Remove(T element) => _items.Remove(element);

  // Додаткове завдання: Збереження у файл
  public void Save(string filePath)
  {
    var options = new JsonSerializerOptions { WriteIndented = true };
    string json = JsonSerializer.Serialize(_items, options);
    File.WriteAllText(filePath, json);
  }

  // Додаткове завдання: Завантаження з файлу
  public void Load(string filePath)
  {
    if (!File.Exists(filePath)) return;

    string json = File.ReadAllText(filePath);
    var loaded = JsonSerializer.Deserialize<List<T>>(json);

    if (loaded != null)
    {
      _items.Clear();
      _items.AddRange(loaded);
    }
  }
}