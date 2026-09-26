using BenchmarkDotNet.Running;

namespace IOU1.BenchmarkTests;

public class Program
{
    static void Main() 
    {
        _ = BenchmarkRunner.Run<PerformanceTests>();
    }   
}
