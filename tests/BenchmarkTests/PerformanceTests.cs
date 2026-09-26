using BenchmarkDotNet.Attributes;

namespace IOU1.BenchmarkTests;

public class PerformanceTests
{
    private readonly List<int> _numbers = Enumerable.Range(1, 1000).ToList();

    [Benchmark]
    public void LinqForEachLoop()
    {
        _numbers.ForEach(n => { _ = n * 2; });
    }

    [Benchmark]
    public void StandardForEachLoop()
    {
        foreach (var item in _numbers)
        {
            _ = item * 2;
        }
    }

    [Benchmark]
    public void ForLoop()
    {
        for(int i = 0; i < _numbers.Count; i++)
        {
            _ = _numbers[i] * 2;
        }
    }
}
