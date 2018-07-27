using System;
using System.Globalization;
using System.Windows.Data;
using FrHello.NetLib.Core.Reflection.Enum;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 后台枚举值和前台枚举描述之间转换
    /// </summary>
    public class EnumDescriptionToEnumValueConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is System.Enum enumValue)
            {
                return enumValue.GetDescription();
            }

            return value;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string enumString && !string.IsNullOrWhiteSpace(enumString) && targetType.IsEnum)
            {
                var enumValues = System.Enum.GetValues(targetType);

                foreach (System.Enum enumValue in enumValues)
                {
                    var description = enumValue.GetDescription();
                    if (enumString == description)
                    {
                        return enumValue;
                    }
                }
            }

            return value;
        }
    }
}
