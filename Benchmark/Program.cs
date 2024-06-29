using BenchmarkDotNet.Running;
using Benchmark;

var summary = BenchmarkRunner.Run<CalculationBenchmark>();
