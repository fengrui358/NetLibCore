using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            var attributes = fieldInfo.GetCustomAttributes();

            foreach (var attribute in attributes)
            {
                var isMatch = false;

                switch (attribute)
                {
                    case EnumDescriptionAttribute enumDescriptionAttribute:
                        description = enumDescriptionAttribute.Description;
                        isMatch = true;
                        break;
                    case DescriptionAttribute descriptionAttribute:
                        description = descriptionAttribute.Description;
                        isMatch = true;
                        break;
                }

                if (isMatch)
                {
                    break;
                }
            }

            return description;
        }

        /// <summary>
        /// 获取枚举对应的特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static TAttribute GetEnumValueAttribute<TAttribute>(this System.Enum enumValue)
            where TAttribute : Attribute
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            return field.GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// 获取枚举对应的特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetEnumValueAttributes<TAttribute>(this System.Enum enumValue)
            where TAttribute : Attribute
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            return field.GetCustomAttributes<TAttribute>();
        }
    }
}
