using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

namespace RestMongoDocker.Kafka
{
  public class ConsumerWrapper
  {
    // List Of Topics?
    public ConsumerConfig _consumerConfig;
    private IEnumerable<string> _topics;
    private string _message { get; set; }

    public ConsumerWrapper(ConsumerConfig consumerConfig)
    {
      this._consumerConfig = consumerConfig;
      this._topics = new List<string> { "testtopic" };
    }

    public string readMessages()
    {
      using (var consumer = new ConsumerBuilder<Ignore, string>(this._consumerConfig).Build())
      {
        // Subscribes to all of the topics in the string array
        consumer.Subscribe(this._topics);

        CancellationTokenSource cts = new CancellationTokenSource();
        // cts.Cancel();

        try
        {
          while (true)
          {
            try
            {
              var cr = consumer.Consume(cts.Token);
              this._message = cr.Value;
            }
            catch (ConsumeException e)
            {
              Console.WriteLine($"Error occured: {e.Error.Reason}");
              this._message = $"Error occured: {e.Error.Reason}";
            }

          }
        }
        catch (OperationCanceledException)
        {
          consumer.Close();
          this._message = "Operation was cancelled";
        }
        return this._message;
      }
    }

  }

}