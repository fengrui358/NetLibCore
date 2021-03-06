﻿using FrHello.NetLib.Core.Security;
using FrHello.NetLib.Core.Serialization;
using Xunit;

namespace NetLib.Core.Test.Security.Test
{
    /// <summary>
    /// HashTest
    /// </summary>
    public class HashTest
    {
        /// <summary>
        /// Md5Test
        /// </summary>
        [Fact]
        // ReSharper disable once InconsistentNaming
        public void Md5Test()
        {
            var str = "123456frefewfe的撒飞洒发fred";
            var expected = "83fadb0e7b9e0d0d6fc3e577b6004333".ToUpper();

            Assert.Equal(expected, SecurityHelper.Hash.Md5.ComputeHash(str));
            Assert.Equal(expected, SecurityHelper.Hash.Md5.ComputeHash(str.ToStream()));
            Assert.Equal(expected, SecurityHelper.Hash.Md5.ComputeHash(str.ToBytes()));
        }

        /// <summary>
        /// Sha1Test
        /// </summary>
        [Fact]
        // ReSharper disable once InconsistentNaming
        public void Sha1Test()
        {
            var str = "123456frefewfe的撒飞洒发fred";
            var expected = "5a0109564c37973b05021eb78c3a3ad3aa918dfe".ToUpper();

            Assert.Equal(expected, SecurityHelper.Hash.Sha1.ComputeHash(str));
            Assert.Equal(expected, SecurityHelper.Hash.Sha1.ComputeHash(str.ToStream()));
            Assert.Equal(expected, SecurityHelper.Hash.Sha1.ComputeHash(str.ToBytes()));
        }

        /// <summary>
        /// Sha256Test
        /// </summary>
        [Fact]
        // ReSharper disable once InconsistentNaming
        public void Sha256Test()
        {
            var str = "123456frefewfe的撒飞洒发fred";
            var expected = "4a461f828d52f844545bafbf7d1c4d5eb5eaaebac231b474ad61e446bfd12a46".ToUpper();

            Assert.Equal(expected, SecurityHelper.Hash.Sha256.ComputeHash(str));
            Assert.Equal(expected, SecurityHelper.Hash.Sha256.ComputeHash(str.ToStream()));
            Assert.Equal(expected, SecurityHelper.Hash.Sha256.ComputeHash(str.ToBytes()));
        }

        /// <summary>
        /// Sha512Test
        /// </summary>
        [Fact]
        // ReSharper disable once InconsistentNaming
        public void Sha512Test()
        {
            var str = "123456frefewfe的撒飞洒发fred";
            var expected =
                "8e3549028575d954c031ecacff28c843c68a9262cbdf47f64cce6d2c6ad856a940e7b2dc9ee6f1739a2d9f1fd913976cee8c54be376cf8557995b046e74eac85"
                    .ToUpper();

            Assert.Equal(expected, SecurityHelper.Hash.Sha512.ComputeHash(str));
            Assert.Equal(expected, SecurityHelper.Hash.Sha512.ComputeHash(str.ToStream()));
            Assert.Equal(expected, SecurityHelper.Hash.Sha512.ComputeHash(str.ToBytes()));
        }
    }
}