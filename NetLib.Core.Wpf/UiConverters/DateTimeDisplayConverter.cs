﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 日期显示转换
    /// </summary>
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeDisplayConverter : MarkupConverter
    {
        private static DateTimeDisplayConverter _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static DateTimeDisplayConverter Instance { get; } =
            _instance ?? (_instance = new DateTimeDisplayConverter());

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
            if (value == null)
            {
                return null;
            }

            if (DateTime.TryParse(value.ToString(), out var date))
            {
                if (date == DateTime.MinValue)
                {
                    //如果是时间的默认值则不显示
                    return DependencyProperty.UnsetValue;
                }

                return date.ToString(DefaultData.DateTimeFormat);
            }

            return null;
        }

        /// <inheritdoc />
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParse(value?.ToString(), out var date))
            {
                return date;
            }

            return DateTime.MinValue;
        }
    }
}