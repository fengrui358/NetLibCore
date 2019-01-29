using System;
using BenchmarkDotNet.Running;
using NetLib.Core.Benchmark.Benchmarks;

namespace NetLib.Core.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            //BenchmarkRunner.Run<HashAlgorithmBenchmark>();
        }
    }
}
