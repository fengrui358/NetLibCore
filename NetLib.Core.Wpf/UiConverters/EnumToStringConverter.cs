using System;
using System.Globalization;
using System.Windows.Data;
using FrHello.NetLib.Core.Reflection.Enum;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 枚举转描述
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Enum @enum)
            {
                return @enum.GetDescription();
            }

            return value;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string enumString && targetType.IsEnum)
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
