using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TFA.Domain.Monitoring;

public class DomainMetrics
{
    private readonly Meter meter = new("TFA.Domain");
    private readonly ConcurrentDictionary<string, Counter<int>> counters = new();
    internal static readonly ActivitySource ActivitySource = new("TFA.Domain");

    public void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = counters.GetOrAdd(name, _ => meter.CreateCounter<int>(name));
        counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }

    public static IDictionary<string, object?> ResultTags(bool success) => new Dictionary<string, object?>
    {
        ["success"] = success
    };
}