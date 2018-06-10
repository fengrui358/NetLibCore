using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FrHello.NetLib.Core.Wpf
{
    /// <summary>
    /// 超级转换提供器
    /// </summary>
    public static class SuperConverterProvider
    {
        /// <summary>
        /// 超级转换
        /// </summary>
        public static SuperConverter SuperConverter = new SuperConverter();

        /// <summary>
        /// 超级转换（反向）
        /// </summary>
        public static SuperConverterInverse SuperConverterInverse = new SuperConverterInverse();
    }

    /// <summary>
    /// 超级转换器
    /// </summary>
    public class SuperConverter : IValueConverter
    {
        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (parameter != null)
            {
                if (value != null && value.Equals(parameter))
                {
                    result = true;
                }
            }
            else
            {
                if (ConverterResultHelper.CanAutoConverterType(targetType))
                {
                    result = true;
                }
                else
                {
                    return value;
                }
            }

            if (result)
            {
                return ConverterResultHelper.GetResult(targetType);

            }
            return ConverterResultHelper.GetResult(targetType, true);
        }

        /// <summary>
        /// 从UI到Model转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 超级转换器(取反)
    /// </summary>
    public class SuperConverterInverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = true;

            if (parameter != null)
            {
                if (value != null && value.Equals(parameter))
                {
                    result = false;
                }
            }
            else
            {
                if (ConverterResultHelper.CanAutoConverterType(targetType))
                {
                    result = false;
                }
                else
                {
                    return value;
                }
            }

            if (result)
            {
                return ConverterResultHelper.GetResult(targetType);

            }
            return ConverterResultHelper.GetResult(targetType, true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 转换结果辅助类
    /// </summary>
    internal static class ConverterResultHelper
    {
        /// <summary>
        /// 根据类型获取结果
        /// </summary>
        /// <param name="resultType">目标类型</param>
        /// <param name="isInverse">是否取反值</param>
        /// <returns></returns>
        internal static object GetResult(Type resultType, bool isInverse = false)
        {
            if (resultType == typeof(bool) || resultType == typeof(bool?))
            {
                return !isInverse;
            }

            if (resultType == typeof(Visibility) || resultType == typeof(Visibility?))
            {
                return isInverse ? Visibility.Collapsed : Visibility.Visible;
            }

            return null;
        }

        /// <summary>
        /// 是可以自动转换结果的类型
        /// </summary>
        /// <param name="resultType"></param>
        /// <returns></returns>
        internal static bool CanAutoConverterType(Type resultType)
        {
            if (resultType == typeof(bool) || resultType == typeof(bool?) || resultType == typeof(Visibility) ||
                resultType == typeof(Visibility?))
            {
                return true;
            }

            return false;
        }
    }
}
