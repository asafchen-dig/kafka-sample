using KafkaFlow;
using Microsoft.Extensions.Hosting;

namespace KafkaSample;

public class KafkaBusHostedService : IHostedService
{
    private readonly IKafkaBus _kafkaBus;

    public KafkaBusHostedService(IServiceProvider serviceProvider)
    {
        _kafkaBus = serviceProvider.CreateKafkaBus();
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _kafkaBus.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _kafkaBus.StopAsync();
    }
}