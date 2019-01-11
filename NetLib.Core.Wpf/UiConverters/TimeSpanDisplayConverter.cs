using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 时间显示转换
    /// </summary>
    public class TimeSpanDisplayConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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