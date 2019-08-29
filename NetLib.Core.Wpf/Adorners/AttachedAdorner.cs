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
        /// <summary>
        /// 附加到其他元素上面的UIElement
        /// </summary>
        protected UIElement AttachedAdornerElement;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="attachedAdornerElement">附加到其他元素上面的UIElement</param>
        /// <param name="adornedTargetElement">需要附加的目标UIElement</param>
        public AttachedAdorner(UIElement attachedAdornerElement, UIElement adornedTargetElement) : this(
            adornedTargetElement)
        {
            AttachedAdornerElement = attachedAdornerElement;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="adornedTargetElement">需要附加的目标UIElement</param>
        protected AttachedAdorner(UIElement adornedTargetElement) : base(adornedTargetElement)
        {
            AddLogicalChild(AttachedAdornerElement);
            AddVisualChild(AttachedAdornerElement);
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
            var desiredSizeHeight = AttachedAdornerElement.DesiredSize.Height + 5;

            //放在目标元素上面
            AttachedAdornerElement.Arrange(new Rect(AdornedElement.RenderSize.Width / 2 - desiredSizeWidth / 2,
                0 - desiredSizeHeight, desiredSizeWidth, desiredSizeHeight));

            return finalSize;
        }

        /// <summary>
        /// VisualChildrenCount
        /// </summary>
        protected override int VisualChildrenCount { get; } = 1;

        /// <summary>
        /// GetVisualChild
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            return AttachedAdornerElement;
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
