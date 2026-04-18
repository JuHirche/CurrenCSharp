using BenchmarkDotNet.Running;
using CurrenCSharp.Benchmark;

BenchmarkSwitcher
    .FromAssembly(typeof(WalletBenchmarks).Assembly)
    .Run(args);
