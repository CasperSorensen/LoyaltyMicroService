using System;
using Shared.Config;
using MongoDB.Driver;

namespace RestMongoDocker.Models
{
  public class UserContext : IUserContext
  {

    private readonly IMongoDatabase _db;

    public UserContext(MongoDbConfig config)
    {
      var client = new MongoClient(config.ConnectionString);
      this._db = client.GetDatabase(config.Database);

    }

    public IMongoCollection<User> Users => _db.GetCollection<User>("Users");

  }
}