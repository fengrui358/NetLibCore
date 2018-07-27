using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using FrHello.NetLib.Core.Reflection.Enum;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 枚举值转描述数据源
    /// </summary>
    public class EnumToItemsSourceConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type enumType && enumType.IsEnum)
            {
                var enumValues = System.Enum.GetValues(enumType);

                var result = new List<string>();
                foreach (System.Enum enumValue in enumValues)
                {
                    result.Add(enumValue.GetDescription());
                }

                return result;
            }

            return Binding.DoNothing;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
