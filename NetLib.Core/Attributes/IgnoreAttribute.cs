using System;

namespace FrHello.NetLib.Core.Attributes
{
    /// <summary>
    /// 忽略
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreAttribute : Attribute
    {
    }
}
