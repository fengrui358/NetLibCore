using FrHello.NetLib.Core.Wpf.Event.EventArgs;

namespace FrHello.NetLib.Core.Wpf.Event
{
    /// <summary>
    /// RoutedPropertyEventHandler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RoutedPropertyEventHandler<T>(object sender, RoutedPropertyEventArgs<T> e);
}
