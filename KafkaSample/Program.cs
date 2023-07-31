using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaSample;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureDefaults(args)
            .ConfigureServices(ConfigureServices)
            .Build();

        host.Run();
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        var topicName = "span-instances";
        
        services.AddKafka(kafka => kafka
            .UseConsoleLog()
            .AddCluster(cluster =>
            {
                cluster
                    .WithBrokers(new[] {"localhost:9092"})
                    .CreateTopicIfNotExists(topicName, 10, -1)
                    .AddConsumer(Consumer<SpanMessageHandler1>(topicName))
                    .AddConsumer(Consumer<SpanMessageHandler2>(topicName))
                    .AddProducer<SpanMessage>(producer => producer
                        .DefaultTopic(topicName)
                        .AddMiddlewares(middlewares => middlewares
                            .AddSerializer<JsonCoreSerializer>()
                        )
                    );
            })
        );

        services.AddSingleton<HistogramService>();
        services.AddHostedService<KafkaBusHostedService>();
        services.AddHostedService<SpanProducerHostedService>();
    }

    private static Action<IConsumerConfigurationBuilder> Consumer<T>(string topicName) where T : class, IMessageHandler
    {
        return consumer => consumer
            .Topic(topicName)
            .WithGroupId("sample-group")
            .WithBufferSize(1)
            .WithWorkersCount(1)
            .AddMiddlewares(middlewares => middlewares
                .AddSerializer<JsonCoreSerializer>()
                .AddTypedHandlers(handlers => handlers
                    .AddHandler<T>())
            );
    }
}