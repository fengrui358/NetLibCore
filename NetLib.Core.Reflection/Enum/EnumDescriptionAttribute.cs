using System;

namespace FrHello.NetLib.Core.Reflection.Enum
{
    /// <summary>
    /// 枚举描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDescriptionAttribute : Attribute
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="description"></param>
        public EnumDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
