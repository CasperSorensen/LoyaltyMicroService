using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using RestMongoDocker.Models;

namespace RestMongoDocker.Repositories
{
  public class TodoRepository : ITodoRepository
  {
    private readonly ITodoContext _context;

    public TodoRepository(ITodoContext context)
    {
      this._context = context;

    }

    // api/[GET]
    public async Task<IEnumerable<Todo>> GetAllTodos()
    {
      return await _context
                    .Todos
                    .Find(_ => true)
                    .ToListAsync();
    }

    // api/where/{id}/equals/{id} /[GET]
    public Task<Todo> GetTodo(long id)
    {
      FilterDefinition<Todo> filter = Builders<Todo>.Filter.Eq(m => m.Id, id);
      return _context
              .Todos
              .Find(filter)
              .FirstOrDefaultAsync();
    }

    // api/[POST]
    public async Task Create(Todo todo)
    {
      await _context
              .Todos
              .InsertOneAsync(todo);

    }

    // api/[PUT]
    public async Task<bool> Update(Todo todo)
    {
      ReplaceOneResult updateResult =
                    await _context
                            .Todos
                            .ReplaceOneAsync(filter: g => g.Id == todo.Id, replacement: todo);

      return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    // api[DELETE]
    public async Task<bool> Delete(long id)
    {
      FilterDefinition<Todo> filter = Builders<Todo>.Filter.Eq(m => m.Id, id);

      DeleteResult deleteResult = await _context
                                          .Todos
                                        .DeleteOneAsync(filter);

      return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<long> GetNextId()
    {
      return await _context
                    .Todos
                    .CountDocumentsAsync(new BsonDocument()) + 1;
    }

  }

}