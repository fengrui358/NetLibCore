using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using FrHello.NetLib.Core.Wpf.Helpers;

namespace FrHello.NetLib.Core.Wpf.Behaviors
{
    /// <summary>
    /// 集合控件鼠标双击，发送双击对象到后端绑定命令
    /// </summary>
    public class ItemsControlMouseDoubleClickBehavior : Behavior<ItemsControl>
    {
        /// <summary>
        /// 命令
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand),
                typeof(ItemsControlMouseDoubleClickBehavior),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// 命令参数
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object),
                typeof(ItemsControlMouseDoubleClickBehavior),
                new FrameworkPropertyMetadata((object) null));

        /// <summary>
        /// 命令参数
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += AssociatedObjectOnMouseLeftButtonDown;
        }

        /// <summary>
        /// 清理
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseLeftButtonDown -= AssociatedObjectOnMouseLeftButtonDown;
        }

        /// <summary>
        /// 鼠标点击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObjectOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Command != null && e.ClickCount == 2)
            {
                var element = e.MouseDevice.DirectlyOver;
                if (element is FrameworkElement frameworkElement && sender is ItemsControl itemsControl)
                {
                    //判断是否是点击的某一项
                    var selectedItem = itemsControl.GetType().GetProperty(nameof(Selector.SelectedItem))
                        ?.GetValue(itemsControl);
                    if (selectedItem != null)
                    {
                        //判断是否是单击的某一项
                        var clickUiItem = itemsControl.ItemContainerGenerator.ContainerFromItem(selectedItem);
                        if (UiTreeHelper.IsChild(clickUiItem, frameworkElement, SearchTreeType.Visual))
                        {
                            Command?.Execute(CommandParameter ?? selectedItem);
                        }
                    }
                }
            }
        }
    }
}