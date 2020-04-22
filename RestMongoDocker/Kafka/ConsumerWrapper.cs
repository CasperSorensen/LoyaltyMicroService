using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace RestMongoDocker.Kafka
{
  public class ConsumerWrapper
  {
    // List Of Topics?
    private ConsumerConfig _consumerConfig;

    public ConsumerWrapper(ConsumerConfig consumerconfig)
    {
      this._consumerConfig = consumerconfig;

    }

    // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //   await Task.Run(() =>
    //   {
    //     while (!stoppingToken.IsCancellationRequested)
    //     {
    //       var message = _consumer.Consume(stoppingToken);

    //     }
    //   }, stoppingToken);
    // }

  }

}