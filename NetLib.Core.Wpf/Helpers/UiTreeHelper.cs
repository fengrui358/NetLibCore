using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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

        /// <summary>
        /// 查找祖先节点上某个指定的节点。
        /// </summary>
        /// <typeparam name="T">要查找的节点类型。</typeparam>
        /// <param name="current">开始查找的当前节点。</param>
        /// <param name="name">可视化对象上的Name</param>
        /// <returns></returns>
        public static T FindAncestorNode<T>(DependencyObject current, string name = "") where T : DependencyObject
        {
            return FindAncestorNode(current, typeof(T), name) as T;
        }

        /// <summary>
        /// 查找祖先节点上某个指定的节点。
        /// </summary>
        /// <param name="current">开始查找的当前节点。</param>
        /// <param name="type">要查找的节点类型。</param>
        /// <param name="name">可视化对象上的Name</param>
        /// <returns></returns>
        public static DependencyObject FindAncestorNode(DependencyObject current, Type type, string name = "")
        {
            if (current == null)
                return null;

            if (current.GetType() == type)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return current;
                }
                else if (current is FrameworkElement frameworkElement && frameworkElement.Name == name)
                {
                    return current;
                }
            }

            var parent = GetParent(current);
            return FindAncestorNode(parent, type, name);
        }

        /// <summary>
        /// 获取父可视对象中第一个指定类型的子可视对象
        /// </summary>
        /// <typeparam name="T">可视对象类型</typeparam>
        /// <param name="parent">父可视对象</param>
        /// <param name="name">可视化对象上的Name</param>
        /// <returns>第一个指定类型的子可视对象</returns>
        public static T GetVisualChild<T>(Visual parent, string name = "") where T : Visual
        {
            T child = default;
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual) VisualTreeHelper.GetChild(parent, i);

                child = v as T ?? GetVisualChild<T>(v);

                if (child != null)
                {
                    //判断Tag标签
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (child is FrameworkElement tagFrameworkElement && tagFrameworkElement.Name != null &&
                            tagFrameworkElement.Name == name)
                        {
                            break;
                        }
                        else
                        {
                            child = GetVisualChild<T>(v, name);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return child;
        }

        /// <summary>
        /// 查找直接父节点
        /// </summary>
        /// <param name="dpObj"></param>
        /// <returns></returns>
        private static DependencyObject GetParent(DependencyObject dpObj)
        {
            if (dpObj == null)
                return null;

            return dpObj is Visual || dpObj is Visual3D
                ? VisualTreeHelper.GetParent(dpObj)
                : LogicalTreeHelper.GetParent(dpObj);
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