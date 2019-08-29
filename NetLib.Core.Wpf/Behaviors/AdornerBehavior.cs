using System;
using System.Windows;
using System.Windows.Documents;
using FrHello.NetLib.Core.Wpf.Adorners;

namespace FrHello.NetLib.Core.Wpf.Behaviors
{
    /// <summary>
    /// AdornerBehavior
    /// </summary>
    public class AdornerBehavior
    {
        /// <summary>
        /// AdornerProperty
        /// </summary>
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner", typeof(UIElement), typeof(AdornerBehavior),
            new PropertyMetadata(default(UIElement), OnAdornerChangedCallback));

        /// <summary>
        /// GetAdorner
        /// </summary>
        /// <param name="element">需要附加的目标UIElement</param>
        /// <returns></returns>
        public static UIElement GetAdorner(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (UIElement)element.GetValue(AdornerProperty);
        }

        /// <summary>
        /// SetAdorner
        /// </summary>
        /// <param name="element">需要附加的目标UIElement</param>
        /// <param name="value">附加到其他元素上面的UIElement</param>
        public static void SetAdorner(UIElement element, UIElement value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(AdornerProperty, value);
        }

        /// <summary>
        /// OnAdornerChangedCallback
        /// </summary>
        /// <param name="dependencyObject">dependencyObject</param>
        /// <param name="args">args</param>
        private static void OnAdornerChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is UIElement element)
            {
                var uiElement = (UIElement)args.NewValue;

                var adornerLayer = AdornerLayer.GetAdornerLayer(element);

                if (adornerLayer != null)
                {
                    var adorners = adornerLayer?.GetAdorners(element);

                    if (adorners != null)
                    {
                        foreach (var adorner in adorners)
                        {
                            if (adorner is AttachedAdorner oldAttachedAdorner)
                            {
                                adornerLayer.Remove(adorner);
                                oldAttachedAdorner.DisconnectChild();
                            }
                        }
                    }

                    if (uiElement != null)
                    {
                        adornerLayer.Add(new AttachedAdorner(uiElement, element));
                    }
                }
            }
        }
    }
}
