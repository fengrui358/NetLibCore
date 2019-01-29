using System.Security.Authentication;
using BenchmarkDotNet.Attributes;

namespace NetLib.Core.Benchmark.Benchmarks
{
    [ClrJob]
    [MonoJob]
    [CoreJob(baseline: true)]
    public class HashAlgorithmBenchmark
    {
        [ParamsAllValues]
        public HashAlgorithmType HashAlgorithmType { get; set; }

        public void ComputeHash()
        {

        }
    }
}
