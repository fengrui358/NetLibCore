namespace FrHello.NetLib.Core.Wpf.Event
{
    /// <summary>
    /// EventArgsToValueConverterProvider
    /// </summary>
    public class EventArgsToValueConverterProvider
    {
        /// <summary>
        /// ConvertToInt
        /// </summary>
        public static RoutedPropertyEventArgsToValueConverter<int> ConvertToInt { get; } = new RoutedPropertyEventArgsToValueConverter<int>();

        /// <summary>
        /// ConvertToString
        /// </summary>
        public static RoutedPropertyEventArgsToValueConverter<string> ConvertToString { get; } = new RoutedPropertyEventArgsToValueConverter<string>();

        /// <summary>
        /// ConvertToDouble
        /// </summary>
        public static RoutedPropertyEventArgsToValueConverter<double> ConvertToDouble { get; } = new RoutedPropertyEventArgsToValueConverter<double>();

        /// <summary>
        /// Converter
        /// </summary>
        public static RoutedPropertyEventArgsToValueConverter<object> Converter { get; } = new RoutedPropertyEventArgsToValueConverter<object>();
    }
}
