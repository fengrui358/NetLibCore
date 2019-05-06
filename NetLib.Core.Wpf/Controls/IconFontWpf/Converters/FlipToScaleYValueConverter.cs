using System;
using System.Globalization;
using System.Windows;
using FrHello.NetLib.Core.Wpf.UiConverters;
using IconFontWpf;

namespace FrHello.NetLib.Core.Wpf.Controls.IconFontWpf.Converters
{
    /// <summary>
    /// ValueConverter which converts the PackIconFlipOrientation enumeration value to ScaleY value of a ScaleTransformation.
    /// </summary>
    /// <seealso cref="T:MahApps.Metro.IconPacks.Converter.MarkupConverter" />
    public class FlipToScaleYValueConverter : MarkupConverter
    {
        private static FlipToScaleYValueConverter _instance;

        /// <summary>
        /// 对象提供
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new FlipToScaleYValueConverter());
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
                case IconFontFlipOrientation.Vertical:
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