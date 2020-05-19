using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace LoyaltyService.Consumer.Kafka
{
  public class ConsumerWrapper
  {
    ConsumerConfig config = new ConsumerConfig
    {
      GroupId = "Loyalty_Consumer",
      BootstrapServers = "kafka:9092",
      AutoOffsetReset = AutoOffsetReset.Earliest
    };

    public ConsumerWrapper()
    {

    }

    public void Consume()
    {
      using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
      {
        consumer.Subscribe("testtopic");

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
              var cr = consumer.Consume(cts.Token);
              Console.WriteLine("LoyaltyService.Consumer: KEY:" + cr.Message.Key + " VALUE: " + cr.Message.Value);
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