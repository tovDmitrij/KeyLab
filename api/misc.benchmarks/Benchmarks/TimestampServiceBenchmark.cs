using BenchmarkDotNet.Attributes;

namespace misc.benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class TimestampServiceBenchmark
    {
        private readonly DateTime EPOCH_TIME = DateTime.UnixEpoch;

        [Benchmark]
        public double GetCurrentUNIXTime_OLD()
        {
            var currentUNIXTime = DateTime.UtcNow.Subtract(EPOCH_TIME);
            return currentUNIXTime.TotalSeconds;
        }
    }
}