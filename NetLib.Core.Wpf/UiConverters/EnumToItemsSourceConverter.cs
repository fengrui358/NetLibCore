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
    [ValueConversion(typeof(System.Enum), typeof(IEnumerable<string>))]
    public class EnumToItemsSourceConverter : MarkupConverter
    {
        private static EnumToItemsSourceConverter _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static EnumToItemsSourceConverter Instance { get; } =
            _instance ?? (_instance = new EnumToItemsSourceConverter());

        /// <summary>
        /// 提供对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        /// <inheritdoc />
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
