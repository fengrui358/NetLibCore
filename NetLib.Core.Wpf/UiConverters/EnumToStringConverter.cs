using System;
using System.Globalization;
using System.Windows.Data;
using FrHello.NetLib.Core.Reflection.Enum;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 枚举转描述
    /// </summary>
    [ValueConversion(typeof(System.Enum), typeof(string))]
    public class EnumToStringConverter : MarkupConverter
    {
        private static EnumToStringConverter _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static EnumToStringConverter Instance { get; } =
            _instance ?? (_instance = new EnumToStringConverter());

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
            if (value is System.Enum @enum)
            {
                return @enum.GetDescription();
            }

            return value;
        }

        /// <inheritdoc />
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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