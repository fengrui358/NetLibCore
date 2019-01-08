using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace FrHello.NetLib.Core.Wpf.Behaviors
{
    /// <summary>
    /// 鼠标进出缩放元素
    /// </summary>
    public class MouseEnterScaleTransformBehavior : Behavior<UIElement>
    {
        private Transform _oldTransform;
        private Point _oldRenderTransformOrigin;

        /// <summary>
        /// 缩放比例，默认为1.05倍
        /// </summary>
        public double Scale { get; set; } = 1.05;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseEnter += AssociatedObjectOnMouseEnter;
            AssociatedObject.MouseLeave += AssociatedObjectOnMouseLeave;
        }

        private void AssociatedObjectOnMouseEnter(object sender, MouseEventArgs e)
        {
            _oldTransform = AssociatedObject.RenderTransform;
            _oldRenderTransformOrigin = AssociatedObject.RenderTransformOrigin;
            AssociatedObject.RenderTransform = new ScaleTransform(Scale, Scale);
            AssociatedObject.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void AssociatedObjectOnMouseLeave(object sender, MouseEventArgs e)
        {
            AssociatedObject.RenderTransformOrigin = _oldRenderTransformOrigin;
            AssociatedObject.RenderTransform = _oldTransform;
        }

        /// <summary>
        /// 移除
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseEnter -= AssociatedObjectOnMouseEnter;
        }
    }
}
