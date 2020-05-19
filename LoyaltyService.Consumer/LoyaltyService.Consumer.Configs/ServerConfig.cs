using System;

namespace LoyaltyService.Consumer.Configs
{
  public class ServerConfig
  {
    public MongoDbConfig MongoDb { get; set; } = new MongoDbConfig();
  }
}