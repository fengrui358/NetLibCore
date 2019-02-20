using BenchmarkDotNet.Running;

namespace NetLib.Core.Benchmark
{
    /// <summary>
    /// https://benchmarkdotnet.org/articles/guides/getting-started.html
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            //BenchmarkRunner.Run<HashAlgorithmBenchmark>();
        }
    }
}
