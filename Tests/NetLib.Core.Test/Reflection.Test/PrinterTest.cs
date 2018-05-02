using FrHello.NetLib.Core.Reflection;
using Xunit;

namespace NetLib.Core.Test.Reflection.Test
{
    public class PrinterTest
    {
        [Fact]
        public void PringTypeTest()
        {
            var type = typeof(MockClass);

            Printer.Output(type);
        }
    }

    internal class MockClass
    {
        
    }
}
