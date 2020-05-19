using System;
using MongoDB.Driver;
using LoyaltyService.Consumer.Models;

namespace LoyaltyService.Consumer.Contexts
{
  public interface IUserContext
  {
    public IMongoCollection<User> Users { get; }

  }

}