using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FrHello.NetLib.Core.Wpf.UiConverters
{
    /// <summary>
    /// 超级转换提供器
    /// </summary>
    public static class SuperConverterProvider
    {
        /// <summary>
        /// 超级转换
        /// </summary>
        public static SuperConverter SuperConverter { get; } = new SuperConverter();

        /// <summary>
        /// 超级转换（反向）
        /// </summary>
        public static SuperConverterInverse SuperConverterInverse { get; } = new SuperConverterInverse();

        /// <summary>
        /// 超级转换
        /// </summary>
        public static SuperConverterForMulti SuperConverterForMulti { get; } = new SuperConverterForMulti();

        /// <summary>
        /// 超级转换（反向）
        /// </summary>
        public static SuperConverterInverseForMulti SuperConverterInverseForMulti { get; } =
            new SuperConverterInverseForMulti();

        /// <summary>
        /// 枚举值转换器
        /// </summary>
        public static EnumToStringConverter EnumToStringConverter { get; } = new EnumToStringConverter();

        /// <summary>
        /// 枚举类型转数据源
        /// </summary>
        public static EnumToItemsSourceConverter EnumToItemsSourceConverter { get; } =
            new EnumToItemsSourceConverter();

        /// <summary>
        /// 日期显示格式转换
        /// </summary>
        public static DateTimeDisplayConverter DateTimeDisplayConverter { get; } = new DateTimeDisplayConverter();

        /// <summary>
        /// 时间显示格式转换
        /// </summary>
        public static TimeSpanDisplayConverter TimeSpanDisplayConverter { get; } = new TimeSpanDisplayConverter();
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
            return ConverterResultHelper.Convert(false, value, targetType, parameter, culture);
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
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// 超级转换器(取反)
    /// </summary>
    public class SuperConverterInverse : IValueConverter
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
            return ConverterResultHelper.Convert(true, value, targetType, parameter, culture);
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
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// 超级转换器
    /// </summary>
    public class SuperConverterForMulti : IMultiValueConverter
    {
        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterResultHelper.Convert(true, values, targetType, parameter, culture);
        }

        /// <summary>
        /// 从UI到Model转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 超级转换器
    /// </summary>
    public class SuperConverterInverseForMulti : IMultiValueConverter
    {
        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterResultHelper.Convert(false, values, targetType, parameter, culture);
        }

        /// <summary>
        /// 从UI到Model转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
        /// <param name="value">要转换的值</param>
        /// <returns></returns>
        internal static object GetResult(Type resultType, bool value)
        {
            if (resultType == typeof(bool) || resultType == typeof(bool?))
            {
                return value;
            }

            if (resultType == typeof(Visibility) || resultType == typeof(Visibility?))
            {
                return value ? Visibility.Visible : Visibility.Collapsed;
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

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="converterValue">将值处理为bool值，在根据真假和目标类型转换为可处理的值</param>
        /// <param name="value">需要处理的值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">比较参数</param>
        /// <param name="culture">文化</param>
        /// <returns></returns>
        internal static object Convert(bool converterValue, object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            //判断结果能否进行转换
            if (CanAutoConverterType(targetType))
            {
                if (parameter != null)
                {
                    //如果parameter是传的一个类型，则进行类型是否相等的比较
                    if (parameter is Type parameterType)
                    {
                        if (value != null && value.GetType() == parameterType)
                        {
                            converterValue = !converterValue;
                        }
                    }

                    //如果参数不等于空则是判断比较
                    if (value != null && value.Equals(parameter))
                    {
                        converterValue = !converterValue;
                    }
                }
                else
                {
                    if (value is bool boolValue)
                    {
                        //如果值为布尔值，则优先进行判断
                        if (boolValue)
                        {
                            converterValue = !converterValue;
                        }
                    }
                    else if (value is IEnumerable enumerableValue)
                    {
                        //如果值为集合则根据数量进行判断
                        var notEmpty = false;

                        foreach (var unused in enumerableValue)
                        {
                            notEmpty = true;
                            break;
                        }

                        if (notEmpty)
                        {
                            converterValue = !converterValue;
                        }
                    }
                    else
                    {
                        //如果参数等于空则进行一些基本判断
                        if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        {
                            converterValue = !converterValue;
                        }

                        if (value == DependencyProperty.UnsetValue)
                        {
                            converterValue = !converterValue;
                        }
                    }
                }

                var result = GetResult(targetType, converterValue);
                if (result != null)
                {
                    return result;
                }
            }

            return value;
        }

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="converterValue">将值处理为bool值，在根据真假和目标类型转换为可处理的值</param>
        /// <param name="values">需要处理的值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">比较参数</param>
        /// <param name="culture">文化</param>
        /// <returns></returns>
        internal static object Convert(bool converterValue, object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            //判断结果能否进行转换
            if (CanAutoConverterType(targetType))
            {
                foreach (var value in values)
                {
                    if (value == null || value == DependencyProperty.UnsetValue)
                    {
                        converterValue = !converterValue;
                        break;
                    }

                    //判断是否是字符串
                    if (value is string str && string.IsNullOrEmpty(str))
                    {
                        converterValue = !converterValue;
                        break;
                    }

                    //判断是否是bool值
                    if (value is bool boolean && !boolean)
                    {
                        converterValue = !converterValue;
                        break;
                    }
                }

                return ConverterResultHelper.GetResult(targetType, converterValue);
            }

            return Binding.DoNothing;
        }
    }
}