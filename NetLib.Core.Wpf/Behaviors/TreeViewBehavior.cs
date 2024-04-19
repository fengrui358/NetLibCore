using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FrHello.NetLib.Core.Wpf.Behaviors
{
    /// <summary>
    /// TreeView行为增强
    /// </summary>
    public class TreeViewBehavior : Behavior<TreeView>
    {
        private readonly EventSetter _treeViewItemEventSetter;
        private List<string> _treeDataContextPropertiesList; 

        /// <summary>
        /// 为虚拟化节点进行相关缓存
        /// </summary>
        private Tuple<TreeViewItem, List<object>, object> _virtualNodesCache;

        /// <summary>
        /// 选中项依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object),
                typeof(TreeViewBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemChanged));

        /// <summary>
        /// 自动展开选中项
        /// </summary>
        public static readonly DependencyProperty ExpandSelectedProperty =
            DependencyProperty.Register(nameof(ExpandSelected), typeof(bool),
                typeof(TreeViewBehavior),
                new FrameworkPropertyMetadata(false));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (TreeViewBehavior)sender;
            if (behavior._modelHandled) return;

            if (behavior.AssociatedObject == null)
                return;

            behavior._modelHandled = true;
            behavior.UpdateAllTreeViewItems();
            behavior._modelHandled = false;
        }

        private bool _modelHandled;

        /// <summary>
        /// 选中项
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// 自动展开选中项
        /// </summary>
        public bool ExpandSelected
        {
            get => (bool)GetValue(ExpandSelectedProperty);
            set => SetValue(ExpandSelectedProperty, value);
        }

        /// <summary>
        /// 构造
        /// </summary>
        public TreeViewBehavior()
        {
            _treeViewItemEventSetter = new EventSetter(
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler(OnTreeViewItemLoaded));
        }

        /// <summary>
        /// 遍历树，自动展开选中项
        /// </summary>
        private void UpdateAllTreeViewItems()
        {
            var treeView = AssociatedObject;

            if (SelectedItem == null)
            {
                //清空选中项
                if (treeView.ItemContainerGenerator.ContainerFromItem(treeView.SelectedItem) is TreeViewItem
                    selectedTreeViewItem)
                {
                    selectedTreeViewItem.IsSelected = false;
                }
            }
            else
            {
                var isMatch = false;
                var allParents = GetAllParents(SelectedItem).ToList();
                if (allParents.Any())
                {
                    //遍历节点与逻辑父节点进行匹配
                    foreach (var item in treeView.Items)
                    {
                        if (treeView.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tvi)
                        {
                            if (!UpdateTreeViewItem(tvi, allParents, SelectedItem, null, out isMatch))
                            {
                                break;
                            }
                        }
                    }
                }

                //遍历所有可见的展开节点
                if (!isMatch)
                {
                    //遍历UI可见的所有节点
                    foreach (var item in treeView.Items)
                    {
                        if (treeView.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tvi)
                        {
                            if (!UpdateTreeViewItem(tvi, SelectedItem, null))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有的节点路径(有可能包含虚拟化节点)
        /// </summary>
        /// <param name="treeViewItem">树节点</param>
        /// <param name="allParents">所有的父对象</param>
        /// <param name="selectedItem">选中节点</param>
        /// <param name="parenTreeViewItem">父级树节点</param>
        /// <param name="match">是否匹配中</param>
        /// <returns>是否继续</returns>
        private bool UpdateTreeViewItem(TreeViewItem treeViewItem, List<object> allParents, object selectedItem, TreeViewItem parenTreeViewItem, out bool match)
        {
            match = false;

            if (treeViewItem != null)
            {
                var treeView = AssociatedObject;

                var parentItemContainerGenerator = parenTreeViewItem == null ? treeView.ItemContainerGenerator : parenTreeViewItem.ItemContainerGenerator;

                if (parentItemContainerGenerator
                        .ContainerFromItem(selectedItem) is TreeViewItem selectedTreeViewItem &&
                    Equals(selectedTreeViewItem, treeViewItem))
                {
                    //已经匹配成功
                    selectedTreeViewItem.IsSelected = true;
                    if (ExpandSelected && selectedTreeViewItem.HasItems && !selectedTreeViewItem.IsExpanded)
                    {
                        selectedTreeViewItem.IsExpanded = true;
                    }

                    //清空缓存
                    _virtualNodesCache = null;

                    match = true;
                    return false;
                }

                if (allParents == null || !allParents.Any())
                {
                    return false;
                }

                object matchObj = null;
                TreeViewItem matchTreeViewItem = null;

                foreach (var parent in allParents)
                {
                    if (Equals(parent, treeViewItem.DataContext))
                    {
                        matchObj = parent;
                        matchTreeViewItem = parentItemContainerGenerator
                            .ContainerFromItem(parent) as TreeViewItem;
                        if (matchTreeViewItem != null && !matchTreeViewItem.IsExpanded)
                        {
                            matchTreeViewItem.IsExpanded = true;
                        }

                        break;
                    }
                }

                if (matchObj != null && matchTreeViewItem != null)
                {
                    allParents.Remove(matchObj);

                    foreach (var item in matchTreeViewItem.Items)
                    {
                        if (matchTreeViewItem.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tvi)
                        {
                            if (!UpdateTreeViewItem(tvi, allParents, SelectedItem, matchTreeViewItem, out match))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            //父级节点匹配成功，但是查找下级节点时发现为空，则证明下级节点还在虚拟化当中，暂存节点信息，等实例化完毕后继续
                            if (_virtualNodesCache == null || _virtualNodesCache.Item3 != selectedItem ||
                                !Equals(_virtualNodesCache.Item1, matchTreeViewItem) ||
                                _virtualNodesCache.Item2 != allParents)
                            {
                                _virtualNodesCache =
                                    new Tuple<TreeViewItem, List<object>, object>(matchTreeViewItem, allParents,
                                        selectedItem);
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 遍历所有可见的节点
        /// </summary>
        /// <param name="item"></param>
        /// <param name="selectedItem"></param>
        /// <param name="parenTreeViewItem"></param>
        /// <param name="isSelected">匹配后是否选中</param>
        /// <returns>是否继续</returns>
        private bool UpdateTreeViewItem(TreeViewItem item, object selectedItem, TreeViewItem parenTreeViewItem, bool isSelected = true)
        {
            if (selectedItem == null) return true;

            var treeView = AssociatedObject;

            if (item != null)
            {
                var parent = parenTreeViewItem == null ? treeView.ItemContainerGenerator : parenTreeViewItem.ItemContainerGenerator;

                if (parent.ContainerFromItem(selectedItem) is TreeViewItem
                        selectedTreeViewItem && Equals(selectedTreeViewItem, item))
                {
                    if (isSelected)
                    {
                        selectedTreeViewItem.IsSelected = true;
                        if (ExpandSelected && selectedTreeViewItem.HasItems && !selectedTreeViewItem.IsExpanded)
                        {
                            selectedTreeViewItem.IsExpanded = true;
                        }
                    }
                    else
                    {
                        selectedTreeViewItem.IsSelected = false;
                    }

                    return false;
                }

                foreach (var subItem in item.Items)
                {
                    if (item.ItemContainerGenerator.ContainerFromItem(subItem) is TreeViewItem tvi)
                    {
                        if (!UpdateTreeViewItem(tvi, SelectedItem, item))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// UI选中项变更，通知Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            if (_modelHandled) return;

            _modelHandled = true;
            SelectedItem = args.NewValue;
            _modelHandled = false;
        }

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            UpdateTreeViewItemStyle();

            if (AssociatedObject.SelectedItem != null || SelectedItem != null)
            {
                _modelHandled = true;
                UpdateAllTreeViewItems();
                _modelHandled = false;
            }
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
        {
            BuildDataContextTreeStruct(AssociatedObject);

            if (AssociatedObject.SelectedItem != null || SelectedItem != null)
            {
                _modelHandled = true;
                UpdateAllTreeViewItems();
                _modelHandled = false;
            }
        }

        /// <summary>
        /// 构建DataContext的树结构
        /// </summary>
        private void BuildDataContextTreeStruct(TreeView treeView)
        {
            if (_treeDataContextPropertiesList == null)
            {
                _treeDataContextPropertiesList = new List<string>();
            }
            else
            {
                _treeDataContextPropertiesList.Clear();
            }

            //从模板中获取孩子节点的属性名
            IList<string> GetChildrenPropertyName(DataTemplate dataTemplate)
            {
                var treeDataContextPropertiesList = new List<string>();

                if (dataTemplate is HierarchicalDataTemplate hierarchicalDataTemplate)
                {
                    //只考虑简单的Binding的情况，暂时未考虑多绑定
                    if (hierarchicalDataTemplate.ItemsSource is Binding binding)
                    {
                        var propertyName = binding.Path.Path;
                        treeDataContextPropertiesList.Add(propertyName);
                    }

                    if (hierarchicalDataTemplate.ItemTemplate != null)
                    {
                        treeDataContextPropertiesList.AddRange(
                            GetChildrenPropertyName(hierarchicalDataTemplate.ItemTemplate));
                    }

                    return treeDataContextPropertiesList;
                }

                return treeDataContextPropertiesList;
            }

            _treeDataContextPropertiesList = GetChildrenPropertyName(treeView.ItemTemplate).ToList();
        }

        /// <summary>
        /// 获取某一项的所有父节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IList<object> GetAllParents(object obj)
        {
            var result = new List<object>();
            if (obj == null || _treeDataContextPropertiesList == null)
            {
                return result;
            }

            foreach (var item in AssociatedObject.ItemsSource)
            {
                if (Equals(item, obj))
                {
                    return result;
                }
                else
                {
                    var finds = GetParent(item, obj);
                    if (finds != null)
                    {
                        result.Add(item);
                        result.AddRange(finds);
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <param name="objSub">子节点</param>
        /// <param name="objSelected">待匹配的选中节点</param>
        /// <param name="layout">层级</param>
        /// <returns>如果为空则没有最终匹配，需要继续寻找，如果不为空则匹配成功</returns>
        IList<object> GetParent(object objSub, object objSelected, int layout = 0)
        {
            if (_treeDataContextPropertiesList.Count < (layout + 1))
            {
                return null;
            }

            if (objSub.GetType().GetProperty(_treeDataContextPropertiesList[layout])
                ?.GetValue(objSub) is IEnumerable children)
            {
                foreach (var child in children)
                {
                    if (Equals(child, objSelected))
                    {
                        return new List<object>();
                    }
                    else
                    {
                        var finds = GetParent(child, objSelected, layout + 1);
                        if (finds != null)
                        {
                            var r = new List<object> {child};
                            r.AddRange(finds);
                            return r;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.ItemContainerStyle?.Setters.Remove(_treeViewItemEventSetter);

                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
                AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
            }
        }

        // Inject Loaded event handler into ItemContainerStyle
        private void UpdateTreeViewItemStyle()
        {
            if (AssociatedObject.ItemContainerStyle == null)
                AssociatedObject.ItemContainerStyle = new Style(
                    typeof(TreeViewItem),
                    Application.Current.TryFindResource(typeof(TreeViewItem)) as Style);

            if (!AssociatedObject.ItemContainerStyle.Setters.Contains(_treeViewItemEventSetter))
                AssociatedObject.ItemContainerStyle.Setters.Add(_treeViewItemEventSetter);
        }

        /// <summary>
        /// 当有新的节点从虚拟化到进行真正加载时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTreeViewItemLoaded(object sender, RoutedEventArgs args)
        {
            if (_virtualNodesCache != null)
            {
                UpdateTreeViewItem((TreeViewItem)sender, _virtualNodesCache.Item2, _virtualNodesCache.Item3,
                    _virtualNodesCache.Item1, out _);
            }
        }
    }
}
