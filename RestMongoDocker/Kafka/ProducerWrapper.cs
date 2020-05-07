using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace RestMongoDocker.Kafka
{
  public class ProducerWrapper
  {
    private string _topicName { get; set; }
    private ProducerConfig _config { get; set; }
    private string _message { get; set; }

    public ProducerWrapper(ProducerConfig config, string topicName)
    {
      this._topicName = topicName;
      this._config = config;
    }

    public async Task WriteMessage(string message)
    {
      this._message = message;

      using (var producer = new ProducerBuilder<Null, string>(this._config)
      .SetKeySerializer(Serializers.Null)
      .SetValueSerializer(Serializers.Utf8)
      .Build())
      {
        var result = await producer.ProduceAsync(this._topicName, new Message<Null, string>()
        {
          Value = this._message
        });

        Console.WriteLine($"Event >> {this._message} << Sent to Topic: {result.Topic}: Partition: {result.Partition} with Offset: {result.Offset}");

        producer.Flush(TimeSpan.FromSeconds(10));
      }
    }

  }

}