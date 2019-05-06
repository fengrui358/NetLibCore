using System;
using System.Globalization;
using System.Windows;
using FrHello.NetLib.Core.Wpf.UiConverters;
using IconFontWpf;

namespace FrHello.NetLib.Core.Wpf.Controls.IconFontWpf.Converters
{
    /// <summary>
    /// ValueConverter which converts the PackIconFlipOrientation enumeration value to ScaleX value of a ScaleTransformation.
    /// </summary>
    /// <seealso cref="T:MahApps.Metro.IconPacks.Converter.MarkupConverter" />
    public class FlipToScaleXValueConverter : MarkupConverter
    {
        private static FlipToScaleXValueConverter _instance;

        /// <summary>
        /// 提供对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new FlipToScaleXValueConverter());
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (!(value is IconFontFlipOrientation))
                return DependencyProperty.UnsetValue;
            int num;
            switch ((IconFontFlipOrientation) value)
            {
                case IconFontFlipOrientation.Horizontal:
                case IconFontFlipOrientation.Both:
                    num = -1;
                    break;
                default:
                    num = 1;
                    break;
            }

            return num;
        }

        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}