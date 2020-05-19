using System;
using LoyaltyService.Consumer.Configs;
using LoyaltyService.Consumer.Contexts;
using LoyaltyService.Consumer.Models;
using MongoDB.Driver;


namespace LoyaltyService.Consumer.Contexts
{
  public class UserContext : IUserContext
  {
    private readonly IMongoDatabase _db;

    public UserContext(MongoDbConfig config)
    {
      var client = new MongoClient(config.ConnectionString);
      _db = client.GetDatabase(config.Database);
    }

    public IMongoCollection<User> Users => _db.GetCollection<User>("Users");

  }
}