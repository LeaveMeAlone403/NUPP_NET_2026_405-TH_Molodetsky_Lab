using MongoDB.Driver;
using University.Infrastructure.Repositories;

namespace University.Nosql;

public class MongoRepository<T> : IRepository<T> where T : class
{
  private readonly IMongoCollection<T> _collection;

  public MongoRepository(string connectionString, string databaseName, string collectionName)
  {
    var client = new MongoClient(connectionString);
    var database = client.GetDatabase(databaseName);
    _collection = database.GetCollection<T>(collectionName);
  }

  public async Task<T?> GetByIdAsync(Guid id)
  {
    // Припускаємо, що у моделі є поле Id типу Guid або воно мапиться як ObjectId
    var filter = Builders<T>.Filter.Eq("Id", id);
    return await _collection.Find(filter).FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _collection.Find(_ => true).ToListAsync();
  }

  public async Task AddAsync(T entity)
  {
    await _collection.InsertOneAsync(entity);
  }

  public async Task Update(T entity)
  {
    // Використовуємо рефлексію або фіксацію полем Id для оновлення
    var idProp = typeof(T).GetProperty("Id");
    if (idProp != null)
    {
      var idValue = (Guid)idProp.GetValue(entity)!;
      var filter = Builders<T>.Filter.Eq("Id", idValue);
      await _collection.ReplaceOneAsync(filter, entity);
    }
  }

  public async Task Delete(T entity)
  {
    var idProp = typeof(T).GetProperty("Id");
    if (idProp != null)
    {
      var idValue = (Guid)idProp.GetValue(entity)!;
      var filter = Builders<T>.Filter.Eq("Id", idValue);
      await _collection.DeleteOneAsync(filter);
    }
  }

  public async Task<int> SaveChangesAsync()
  {
    // MongoDB зберігає документи атомарно одразу при виклику методів Insert/Update/Delete,
    // тому тут просто повертаємо 1 як успішну операцію.
    return await Task.FromResult(1);
  }
}