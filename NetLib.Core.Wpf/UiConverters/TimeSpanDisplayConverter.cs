using System;
using System.Globalization;
using System.Windows;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 时间显示转换
    /// </summary>
    public class TimeSpanDisplayConverter : MarkupConverter
    {
        private static TimeSpanDisplayConverter _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static TimeSpanDisplayConverter Instance { get; } =
            _instance ?? (_instance = new TimeSpanDisplayConverter());

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
            TimeSpan timeSpanValue = TimeSpan.MinValue;

            if (value is TimeSpan timeSpan)
            {
                timeSpanValue = timeSpan;
            }
            else if (value is double)
            {
            }

            if (timeSpanValue != TimeSpan.MinValue)
            {
            }

            return DependencyProperty.UnsetValue;
        }

        /// <inheritdoc />
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && TimeSpan.TryParse(value.ToString(), out var timeSpanValue))
            {
                if (targetType == typeof(double) || targetType == typeof(double?))
                {
                    return timeSpanValue.TotalSeconds;
                }
                else if (targetType == typeof(TimeSpan))
                {
                    return timeSpanValue;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}