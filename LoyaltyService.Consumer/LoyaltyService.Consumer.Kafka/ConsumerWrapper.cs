using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using LoyaltyService.Consumer.Repositories;
using LoyaltyService.Consumer.Contexts;
using LoyaltyService.Consumer.Configs;
using Newtonsoft.Json;

namespace LoyaltyService.Consumer.Kafka
{
  public class ConsumerWrapper
  {
    ConsumerConfig config = new ConsumerConfig
    {
      GroupId = "loyalty_consumer",
      BootstrapServers = "kafka:9092",
      AutoOffsetReset = AutoOffsetReset.Earliest
    };

    private IEnumerable<string> _topics = new List<string>() { "testtopic" };
    private ServerConfig _mongoconfig { get; set; }
    private UserContext _usercontext { get; set; }
    private UserRepository _userrepo { get; set; }

    public ConsumerWrapper()
    {
      this._mongoconfig = new ServerConfig();
      this._usercontext = new UserContext(this._mongoconfig.MongoDb);
      this._userrepo = new UserRepository(this._usercontext);
    }

    public void Consume()
    {
      using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
      {
        //consumer.Subscribe("testtopic");
        consumer.Subscribe(this._topics);

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
          e.Cancel = true;
          cts.Cancel();
        };
        try
        {
          while (true)
          {
            try
            {
              var consumerResult = consumer.Consume(cts.Token);
              Console.WriteLine("LoyaltyService.Consumer:" + consumerResult.Message.Value);
            }
            catch (ConsumeException e)
            {
              Console.WriteLine($"Error occured: {e.Error.Reason}");
            }
          }
        }
        catch (OperationCanceledException)
        {
          consumer.Close();
        }
      }
    }

  }
}