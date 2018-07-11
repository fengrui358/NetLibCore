using System.Windows;
using System.Windows.Media;

namespace FrHello.NetLib.Core.Wpf.Helpers
{
    /// <summary>
    /// Ui树的辅助类
    /// </summary>
    public static class UiTreeHelper
    {
        /// <summary>
        /// 判断一个元素是否是另一个元素的本身或子元素
        /// </summary>
        /// <param name="parent">父元素</param>
        /// <param name="child">子元素</param>
        /// <param name="searchTreeType">搜索类型</param>
        /// <returns></returns>
        public static bool IsChild(DependencyObject parent, DependencyObject child,
            SearchTreeType searchTreeType = SearchTreeType.Logic)
        {
            if (parent == null || child == null)
            {
                return false;
            }

            if (Equals(parent, child))
            {
                return true;
            }

            if (searchTreeType == SearchTreeType.Logic)
            {
                foreach (var childItem in LogicalTreeHelper.GetChildren(parent))
                {
                    if (childItem is DependencyObject dependencyObject &&
                        IsChild(dependencyObject, child, searchTreeType))
                    {
                        return true;
                    }
                }
            }
            else if (searchTreeType == SearchTreeType.Visual)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var childItem = VisualTreeHelper.GetChild(parent, i);
                    if (childItem is DependencyObject dependencyObject &&
                        IsChild(dependencyObject, child, searchTreeType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    /// <summary>
    /// 查找模式
    /// </summary>
    public enum SearchTreeType
    {
        /// <summary>
        /// 逻辑树
        /// </summary>
        Logic,

        /// <summary>
        /// 可视化树
        /// </summary>
        Visual
    }
}
