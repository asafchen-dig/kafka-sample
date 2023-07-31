using KafkaFlow;
using KafkaFlow.TypedHandler;

namespace KafkaSample;

public class BaseSpanMessageHandler : IMessageHandler<SpanMessage>
{
    private readonly HistogramService _histogramService;

    public BaseSpanMessageHandler(HistogramService histogramService)
    {
        _histogramService = histogramService;
    }

    public Task Handle(IMessageContext context, SpanMessage message)
    {
        _histogramService.Inc(message.Id);
        return Task.CompletedTask;
    }
}

public class SpanMessageHandler1 : BaseSpanMessageHandler
{
    public SpanMessageHandler1(HistogramService histogramService) : base(histogramService)
    {
    }
}

public class SpanMessageHandler2 : BaseSpanMessageHandler
{
    public SpanMessageHandler2(HistogramService histogramService) : base(histogramService)
    {
    }
}