using System;
using System.Reflection;

namespace FrHello.NetLib.Core.Reflection.Enum
{
    /// <summary>
    /// 枚举辅助类
    /// </summary>
    public static class EnumReflectionHelper
    {
        /// <summary>
        /// 获取枚举方法的描述文本
        /// </summary>
        /// <param name="enumWithDescription"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum enumWithDescription)
        {
            if (enumWithDescription == null)
            {
                throw new ArgumentNullException(nameof(enumWithDescription));
            }

            var description = enumWithDescription.ToString();
            var fieldInfo = enumWithDescription.GetType().GetField(description);
            var attribute =
                (EnumDescriptionAttribute) fieldInfo.GetCustomAttribute(typeof(EnumDescriptionAttribute), false);

            if (attribute != null)
            {
                description = attribute.Description;
            }

            return description;
        }
    }
}
