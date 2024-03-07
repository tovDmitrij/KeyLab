#define sec
using BenchmarkDotNet.Running;

using misc.benchmarks;

#if valid
BenchmarkRunner.Run<RegexHelperBenchmark>();

#elif sec
BenchmarkRunner.Run<SecurityHelperBenchmark>();

#endif