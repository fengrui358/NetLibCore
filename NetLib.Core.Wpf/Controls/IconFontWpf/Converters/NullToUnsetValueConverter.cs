using System;
using System.Globalization;
using System.Windows;
using FrHello.NetLib.Core.Wpf.UiConverters;

namespace FrHello.NetLib.Core.Wpf.Controls.IconFontWpf.Converters
{
    /// <summary>
    /// 空值转换
    /// </summary>
    public class NullToUnsetValueConverter : MarkupConverter
    {
        private static NullToUnsetValueConverter _instance;

        /// <summary>
        /// 对象提供
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ??= new NullToUnsetValueConverter();
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}