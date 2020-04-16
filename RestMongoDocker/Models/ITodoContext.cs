using System;
using MongoDB.Driver;

namespace RestMongoDocker.Models
{
  public interface ITodoContext
  {
    public IMongoCollection<Todo> Todos { get; }

  }

}