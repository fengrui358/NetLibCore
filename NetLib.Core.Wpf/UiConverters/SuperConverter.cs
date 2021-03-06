﻿using System;
using System.Collections;
using System.Globalization;
using System.Linq;
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
        public static SuperConverter SuperConverter { get; } = SuperConverter.Instance;

        /// <summary>
        /// 超级转换（反向）
        /// </summary>
        public static SuperConverterInverse SuperConverterInverse { get; } = SuperConverterInverse.Instance;

        /// <summary>
        /// 超级转换
        /// </summary>
        public static SuperConverterForMulti SuperConverterForMulti { get; } = SuperConverterForMulti.Instance;

        /// <summary>
        /// 超级转换（反向）
        /// </summary>
        public static SuperConverterInverseForMulti SuperConverterInverseForMulti { get; } =
            SuperConverterInverseForMulti.Instance;

        /// <summary>
        /// 枚举值转换器
        /// </summary>
        public static EnumToStringConverter EnumToStringConverter { get; } = EnumToStringConverter.Instance;

        /// <summary>
        /// 枚举类型转数据源
        /// </summary>
        public static EnumToItemsSourceConverter EnumToItemsSourceConverter { get; } =
            EnumToItemsSourceConverter.Instance;

        /// <summary>
        /// 日期显示格式转换
        /// </summary>
        public static DateTimeDisplayConverter DateTimeDisplayConverter { get; } = DateTimeDisplayConverter.Instance;

        /// <summary>
        /// 时间显示格式转换
        /// </summary>
        public static TimeSpanDisplayConverter TimeSpanDisplayConverter { get; } = TimeSpanDisplayConverter.Instance;
    }

    /// <summary>
    /// 超级转换器
    /// </summary>
    public class SuperConverter : MarkupConverter
    {
        private static SuperConverter _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static SuperConverter Instance { get; } =
            _instance ??= new SuperConverter();

        /// <summary>
        /// 提供对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterResultHelper.Convert(false, value, targetType, parameter, culture);
        }
    }

    /// <summary>
    /// 超级转换器(取反)
    /// </summary>
    public class SuperConverterInverse : MarkupConverter
    {
        private static SuperConverterInverse _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static SuperConverterInverse Instance { get; } =
            _instance ??= new SuperConverterInverse();

        /// <summary>
        /// 提供对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterResultHelper.Convert(true, value, targetType, parameter, culture);
        }
    }

    /// <summary>
    /// 超级转换器
    /// </summary>
    public class SuperConverterForMulti : MarkupMultiConverter
    {
        private static SuperConverterForMulti _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static SuperConverterForMulti Instance { get; } =
            _instance ??= new SuperConverterForMulti();

        /// <summary>
        /// 提供对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
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
        protected override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(s => ConverterResultHelper.Convert(true, value, s, parameter, culture)).ToArray();
        }
    }

    /// <summary>
    /// 超级转换器
    /// </summary>
    public class SuperConverterInverseForMulti : MarkupMultiConverter
    {
        private static SuperConverterInverseForMulti _instance;

        /// <summary>
        /// 对象
        /// </summary>
        public static SuperConverterInverseForMulti Instance { get; } =
            _instance ??= new SuperConverterInverseForMulti();

        /// <summary>
        /// 提供对象
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        /// <summary>
        /// 从Model到UI转换
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
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
        protected override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes.Select(s => ConverterResultHelper.Convert(false, value, s, parameter, culture)).ToArray();
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
            //如果TargetType的类型是Object，则该转换器很可能是被用在了多值转换器中，强制返回bool结果
            if (targetType == typeof(object))
            {
                targetType = typeof(bool);
            }

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
                    else if (value is Visibility visibility)
                    {
                        //如果值为可见性枚举
                        if (visibility == Visibility.Visible)
                        {
                            converterValue = !converterValue;
                        }
                    }
                    else if (value is string str)
                    {
                        if (!string.IsNullOrEmpty(str))
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
                        if (value != null && value != DependencyProperty.UnsetValue)
                        {
                            if (double.TryParse(value.ToString(), out double d))
                            {
                                //如果能够转换为数值类型，进一步判断数值不为0
                                if (Math.Abs(d) > double.Epsilon)
                                {
                                    converterValue = !converterValue;
                                }
                            }
                            else
                            {
                                converterValue = !converterValue;
                            }
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
                    var converterOutValue = (bool)Convert(!converterValue, value, typeof(bool), null, culture);
                    if (!converterOutValue)
                    {
                        converterValue = !converterValue;
                        break;
                    }
                }

                return GetResult(targetType, converterValue);
            }

            return Binding.DoNothing;
        }
    }
}