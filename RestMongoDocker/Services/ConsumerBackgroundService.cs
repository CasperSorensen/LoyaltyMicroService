using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using RestMongoDocker.Kafka;
using RestMongoDocker.Models;

namespace RestMongoDocker.Services
{
  public class ConsumerBackgroundService : BackgroundService
  {
    private ConsumerConfig _consumerConfig;
    private ProducerConfig _producerConfig;

    private delegate void Method(string request);

    public ConsumerBackgroundService(ConsumerConfig consumerConfig, ProducerConfig producerConfig)
    {
      this._consumerConfig = consumerConfig;
      this._producerConfig = producerConfig;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      Console.WriteLine("ConsumerBackgroundService Started");

      while (!stoppingToken.IsCancellationRequested)
      {
        // create a new wrapper with the consumer config
        var consumerWrapper = new ConsumerWrapper(this._consumerConfig);
        var producerWrapper = new ProducerWrapper(this._producerConfig, "ResponseTopic");

        // reading the messages from the wrapper
        string orderRequest = consumerWrapper.readMessages();

        switch (orderRequest)
        {
          case "testtopic":
            Console.WriteLine("Test");
            Execute(Print, orderRequest);
            // producerWrapper.WriteMessage(orderRequest);
            break;
          case "ggtttt":
            Console.WriteLine("Test");
            Execute(Calc2, orderRequest);
            break;

          default:
            Console.WriteLine("Test");
            // producerWrapper.WriteMessage(orderRequest);
            break;
        }
      }
    }

    private void Execute(Method Operation, string request)
    {
      Operation(request);

    }

    public void Print(string request)
    {
      Console.WriteLine(request);

    }

    public void Calc2(string request)
    {

    }
  }

}