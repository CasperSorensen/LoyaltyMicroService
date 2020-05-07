using System;
using MongoDB.Driver;

namespace RestMongoDocker.Models
{
  public interface IUserContext
  {
    public IMongoCollection<User> Users { get; }

  }

}