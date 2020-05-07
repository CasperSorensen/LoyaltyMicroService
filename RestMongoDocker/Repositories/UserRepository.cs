using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using RestMongoDocker.Models;

namespace RestMongoDocker.Repositories
{
  //TODO User replace Users with Users
  public class UserRepository : IUserRepository
  {
    private readonly IUserContext _context;

    public UserRepository(IUserContext context)
    {
      this._context = context;
    }

    // api/[GET]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
      return await _context
                    .Users
                    .Find(_ => true)
                    .ToListAsync();
    }

    // api/where/{id}/equals/{id} /[GET]
    public Task<User> GetUser(long id)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(m => m.Id, id);
      return _context
              .Users
              .Find(filter)
              .FirstOrDefaultAsync();
    }

    // api/[POST]
    public async Task Create(User User)
    {
      await _context
              .Users
              .InsertOneAsync(User);

    }

    // api/[PUT]
    public async Task<bool> Update(User User)
    {
      ReplaceOneResult updateResult =
                    await _context
                            .Users
                            .ReplaceOneAsync(filter: g => g.Id == User.Id, replacement: User);

      return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    // api[DELETE]
    public async Task<bool> Delete(long id)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(m => m.Id, id);

      DeleteResult deleteResult = await _context
                                          .Users
                                        .DeleteOneAsync(filter);

      return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<long> GetNextId()
    {
      return await _context
                    .Users
                    .CountDocumentsAsync(new BsonDocument()) + 1;
    }

  }

}