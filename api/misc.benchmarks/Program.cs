#define sec
using BenchmarkDotNet.Running;

using misc.benchmarks;

#if valid
BenchmarkRunner.Run<ValidationServiceBenchmark>();

#elif sec
BenchmarkRunner.Run<SecurityServiceBenchmark>();

#endif