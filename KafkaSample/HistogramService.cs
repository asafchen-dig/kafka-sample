using System.Collections.Concurrent;
using KafkaFlow;

namespace KafkaSample;

public class HistogramService
{
    private readonly ConcurrentDictionary<string, Histogram> _histograms;

    public HistogramService()
    {
        _histograms = new ConcurrentDictionary<string, Histogram>();
    }

    public void Inc(int messageId)
    {
        var key = messageId.ToString();
        var histogram = _histograms.GetOrAdd(key, _ => new Histogram("H"+key));

        lock (histogram)
        {
            histogram.Counter++;
        }
    }

    private record Histogram(string Name)
    {
        public int Counter { get; set; }
    }
}