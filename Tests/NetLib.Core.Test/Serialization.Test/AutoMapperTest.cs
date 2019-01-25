using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using FrHello.NetLib.Core.Serialization;
using Xunit;

namespace NetLib.Core.Test.Serialization.Test
{
    /// <summary>
    /// 单元测试
    /// </summary>
    public class AutoMapperTest
    {
        /// <summary>
        /// AutoMapperTest
        /// </summary>
        [Fact]
        public void MapTest()
        {
            var sources = new List<MockClassDto>
            {
                new MockClassDto
                {
                    MockItems = new[] {new MockItemClassDto {Str = "A"}, new MockItemClassDto {Str = "B"}}
                }
            };

            var dest = AutoMapperHelper.GetDefaultMapper(new[] {typeof(AutoMapperHelper).Assembly})
                .Map<ObservableCollection<MockClass>>(sources);

            Assert.Equal(sources.Count, dest.Count);
            Assert.NotSame(sources, dest);
            Assert.NotSame(sources[0], dest[0]);
            Assert.Equal(sources[0].MockItems.Count(), dest[0].MockItems.Count);
            Assert.Equal(sources[0].MockItems.Last().Str, dest[0].MockItems.Last().Str);
            Assert.Equal(sources[0].MockItems.First().Str, dest[0].MockItems.First().Str);
            Assert.NotSame(sources[0].MockItems.Last(), dest[0].MockItems.Last());
            Assert.NotSame(sources[0].MockItems.First(), dest[0].MockItems.First());
        }
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class MockClass
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public ObservableCollection<MockItemClass> MockItems { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class MockClassDto
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public IEnumerable<MockItemClassDto> MockItems { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class MockItemClass
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public string Str { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class MockItemClassDto
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public string Str { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}