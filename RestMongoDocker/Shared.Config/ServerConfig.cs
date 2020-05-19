using System;

namespace Shared.Config
{
  public class ServerConfig
  {
    public MongoDbConfig MongoDb { get; set; } = new MongoDbConfig();
  }
}