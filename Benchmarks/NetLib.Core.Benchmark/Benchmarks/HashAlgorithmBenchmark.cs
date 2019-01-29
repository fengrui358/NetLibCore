using BenchmarkDotNet.Attributes;
using FrHello.NetLib.Core.Security;

namespace NetLib.Core.Benchmark.Benchmarks
{
    //[ClrJob]
    //[MonoJob("Mono x64", @"C:\Program Files\Mono\bin\mono.exe")]
    [CoreJob(baseline: true)]
    public class HashAlgorithmBenchmark
    {
        private const string TestStr1 = "The [Arguments] allows you to provide a set of values. Every value must be a compile-time constant (it's C# lanugage limitation for attributes in general). You can also combine [Arguments] with [Params]. As a result, you will get results for each combination of params values.";

        private const string TestStr2 = "2019年1月19日，微软技术（苏州）俱乐部成立，我受邀在成立大会上作了一个名为《ASP.NET Core框架揭秘》的分享。在此次分享中，我按照ASP.NET Core自身的运行原理和设计思想创建了一个 “迷你版” 的ASP.NET Core框架，并且利用这个 “极简” 的模拟框架阐述了ASP.NET Core框架最核心、最本质的东西。整个框架涉及到的核心代码不会超过200行，涉及到7个核心的对象。";

        [Benchmark]
        public void Md5ComputeHash()
        {
            SecurityHelper.Hash.Md5.ComputeHash(TestStr1);
            SecurityHelper.Hash.Md5.ComputeHash(TestStr2);
        }

        [Benchmark(Baseline = true)]
        public void Sha1ComputeHash()
        {
            SecurityHelper.Hash.Sha1.ComputeHash(TestStr1);
            SecurityHelper.Hash.Sha1.ComputeHash(TestStr2);
        }

        [Benchmark]
        public void Sha256ComputeHash()
        {
            SecurityHelper.Hash.Sha256.ComputeHash(TestStr1);
            SecurityHelper.Hash.Sha256.ComputeHash(TestStr2);
        }

        [Benchmark]
        public void Sha384ComputeHash()
        {
            SecurityHelper.Hash.Sha384.ComputeHash(TestStr1);
            SecurityHelper.Hash.Sha384.ComputeHash(TestStr2);
        }

        [Benchmark]
        public void Sha512ComputeHash()
        {
            SecurityHelper.Hash.Sha512.ComputeHash(TestStr1);
            SecurityHelper.Hash.Sha512.ComputeHash(TestStr2);
        }
    }
}