using System;

namespace LoyaltyService.Consumer.Configs
{
  public class MongoDbConfig
  {
    private string _database { get; set; }
    private string _host { get; set; }
    private string _user { get; set; }
    private string _password { get; set; }
    private int _port { get; set; }

    public MongoDbConfig()
    {
      this._database = "UserDb";
      this._host = "localhost";
      this._port = 27017;
      this._user = "root";
      this._password = "example";
    }

    public string ConnectionString
    {
      get
      {
        if (string.IsNullOrEmpty(this._user) || string.IsNullOrEmpty(this._password))
        //return $@"MongoDb://{this._host}:{this._port}";
        {
          return $@"MongoDb://{this._host}:{this._port}";
        }
        return $@"mongodb://{this._user}:{this._password}@{this._host}:{this._port}";

      }
    }

    public string Database
    {
      get
      {
        return this._database;
      }
    }

  }
}