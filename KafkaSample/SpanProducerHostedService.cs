using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.Extensions.Hosting;

namespace KafkaSample;

public class SpanProducerHostedService : BackgroundService
{
    private readonly IMessageProducer<SpanMessage> _producer;
    
    public SpanProducerHostedService(IMessageProducer<SpanMessage> producer)
    {
        _producer = producer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rand = new Random();
        for (var i = 0; i < 100; i++)
        {
            await _producer.ProduceAsync(rand.Next(10).ToString(), new SpanMessage(i));
        }
    }
}