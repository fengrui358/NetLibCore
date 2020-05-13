using System;
using FrHello.NetLib.Core.Reflection;
using FrHello.NetLib.Core.Wpf.Event.EventArgs;

namespace FrHello.NetLib.Core.Wpf.Event
{
    /// <summary>
    /// RoutedPropertyEventArgsToValueConverter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RoutedPropertyEventArgsToValueConverter<T> : IEventArgsConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object Convert(object value, object parameter)
        {
            try
            {
                var valueType = value.GetType();

                if (valueType.GetGenericTypeDefinition() == typeof(RoutedPropertyEventArgs<>))
                {
                    if (typeof(T) == typeof(object) && parameter is Type type)
                    {
                        return TypeHelper.TryChangeType(((dynamic) value).Value, type);
                    }
                    else
                    {
                        return TypeHelper.TryChangeType(((dynamic) value).Value, typeof(T));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return value;
        }
    }
}