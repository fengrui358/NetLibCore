using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Size = System.Windows.Size;

namespace FrHello.NetLib.Core.Wpf.Adorners
{
    /// <summary>
    /// 附加到其他元素上面的Adorner
    /// </summary>
    public class AttachedAdorner : Adorner
    {
        private readonly VisualCollection _visualChilderns;

        /// <summary>
        /// 附加到其他元素上面的UIElement
        /// </summary>
        protected UIElement AttachedAdornerElement;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="attachedAdornerElement">附加到其他元素上面的UIElement</param>
        /// <param name="adornedTargetElement">需要附加的目标UIElement</param>
        public AttachedAdorner(UIElement attachedAdornerElement, UIElement adornedTargetElement) : base(
            adornedTargetElement)
        {
            _visualChilderns = new VisualCollection(this);
            AttachedAdornerElement = attachedAdornerElement;

            AddLogicalChild(AttachedAdornerElement);
            _visualChilderns.Add(AttachedAdornerElement);
        }

        /// <summary>
        /// MeasureOverride
        /// </summary>
        /// <param name="constraint">constraint</param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            AttachedAdornerElement.Measure(constraint);
            return AttachedAdornerElement.DesiredSize;
        }

        /// <summary>
        /// ArrangeOverride
        /// </summary>
        /// <param name="finalSize">finalSize</param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var desiredSizeWidth = AttachedAdornerElement.DesiredSize.Width;
            var desiredSizeHeight = AttachedAdornerElement.DesiredSize.Height;

            //默认为目标元素中心
            var rect = new Rect(AdornedElement.RenderSize.Width / 2 - desiredSizeWidth / 2,
                AdornedElement.RenderSize.Height / 2 - desiredSizeHeight / 2, desiredSizeWidth, desiredSizeHeight);

            if (AttachedAdornerElement is FrameworkElement frameworkElement)
            {
                if (frameworkElement.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    rect.X = AdornedElement.RenderSize.Width - desiredSizeWidth;
                }

                if (frameworkElement.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    rect.X = 0;
                }

                if (frameworkElement.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    rect.Y = AdornedElement.RenderSize.Height - desiredSizeHeight;
                }

                if (frameworkElement.VerticalAlignment == VerticalAlignment.Top)
                {
                    rect.Y = 0;
                }
            }
            
            AttachedAdornerElement.Arrange(rect);

            return finalSize;
        }

        /// <summary>
        /// VisualChildrenCount
        /// </summary>
        protected override int VisualChildrenCount => _visualChilderns.Count;

        /// <summary>
        /// GetVisualChild
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            return _visualChilderns[index];
        }

        /// <summary>
        /// LogicalChildren
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                var list = new ArrayList { AttachedAdornerElement };
                return list.GetEnumerator();
            }
        }

        /// <summary>
        /// DisconnectChild
        /// </summary>
        public void DisconnectChild()
        {
            RemoveLogicalChild(AttachedAdornerElement);
            RemoveVisualChild(AttachedAdornerElement);
        }
    }
}
