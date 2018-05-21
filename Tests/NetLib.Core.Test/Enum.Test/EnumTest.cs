using System;
using FrHello.NetLib.Core.Enum;
using FrHello.NetLib.Core.Reflection.Enum;
using Xunit;

namespace NetLib.Core.Test.Enum.Test
{
    /// <summary>
    /// EnumTest
    /// </summary>
    public class EnumTest
    {
        /// <summary>
        /// EnumDescriptionTest
        /// </summary>
        [Fact]
        public void EnumDescriptionTest()
        {
            var ma = new MockClass{EnumTest = TestEnum.A};
            var mb = new MockClass{EnumTest = TestEnum.B};

            var a = ma.EnumTest.GetDescription();
            var b = mb.EnumTest.GetDescription();

            Assert.Equal("ADescription", a);
            Assert.Equal(mb.EnumTest.ToString(), b);
        }

        /// <summary>
        /// EnumCombineTest
        /// </summary>
        [Fact]
        public void EnumCombineTest_ThrowException()
        {
            Assert.Throws<ArgumentException>(() => {
                var create = Permission.Create;
                create.Combine(Permission.Delete);
            });
            
            Assert.Throws<ArgumentException>(() => {
                var create = Permission.Create;
                create.Combine(PermissionWithFlag.Delete);
            });
        }

        /// <summary>
        /// EnumCombineTest
        /// </summary>
        [Fact]
        public void EnumCombineTest()
        {
            var create = PermissionWithFlag.Create;
            var excepted = create | PermissionWithFlag.Update;
            
            var actual = create.Combine(PermissionWithFlag.Update);
            Assert.Equal(excepted, actual);

            Assert.Equal($"{PermissionWithFlag.Create}, {PermissionWithFlag.Update}", actual.ToString());
        }

        /// <summary>
        /// EnumCombineTest
        /// </summary>
        [Fact]
        public void EnumCombinedContainsTest()
        {
            var noFlags = Permission.Delete.Combine(false, Permission.Update);
            var noFlagsNotContains = noFlags.Contains(false, Permission.Create);

            Assert.False(noFlagsNotContains);

            var noFlagsContains = noFlags.Contains(false, Permission.Delete);
            Assert.True(noFlagsContains);

            var flags = PermissionWithFlag.Delete.Combine(PermissionWithFlag.Update);
            var flagsNotContains = flags.Contains(PermissionWithFlag.Create);

            Assert.False(flagsNotContains);

            var flagsContains = flags.Contains(PermissionWithFlag.Delete);
            Assert.True(flagsContains);
        }

        /// <summary>
        /// EnumCombineTest
        /// </summary>
        [Fact]
        public void EnumRemoveTest()
        {
            //todo
            //var noFlags = Permission.Delete.Combine(false, Permission.Update);
            //var noFlagsDelete = noFlags.Remove(false, Permission.Update);

            //Assert.Equal(Permission.Delete, noFlagsDelete);

            //var flags = PermissionWithFlag.Delete.Combine(PermissionWithFlag.Update);
            //var flagsDelete = flags.Remove(false, PermissionWithFlag.Update);

            //Assert.Equal(PermissionWithFlag.Delete, flagsDelete);
        }

        /// <summary>
        /// MockClass
        /// </summary>
        public class MockClass
        {
            /// <summary>
            /// EnumTest
            /// </summary>
            public TestEnum EnumTest { get; set; }
        }

        /// <summary>
        /// TestEnum
        /// </summary>
        public enum TestEnum
        {
            /// <summary>
            /// A
            /// </summary>
            [EnumDescription("ADescription")]
            A,

            /// <summary>
            /// B
            /// </summary>
            B
        }

        /// <summary>
        /// Permission
        /// </summary>
        public enum Permission
        {
            /// <summary>
            /// Create
            /// </summary>
            Create = 1,

            /// <summary>
            /// Read
            /// </summary>
            Read = 2,

            /// <summary>
            /// Update
            /// </summary>
            Update = 4,

            /// <summary>
            /// Delete
            /// </summary>
            Delete = 8
        }

        /// <summary>
        /// Permission
        /// </summary>
        [Flags]
        public enum PermissionWithFlag
        {
            /// <summary>
            /// Create
            /// </summary>
            Create = 0b0000_0001,

            /// <summary>
            /// Read
            /// </summary>
            Read = 0b0000_0010,

            /// <summary>
            /// Update
            /// </summary>
            Update = 0b0000_0100,

            /// <summary>
            /// Delete
            /// </summary>
            Delete = 0b0000_1000,
        }
    }
}
