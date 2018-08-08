using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FrHello.NetLib.Core.Mvx.Interfaces;
using MvvmCross.Commands;

namespace FrHello.NetLib.Core.Mvx
{
    /// <summary>
    /// 带列表的基础ViewModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseEditListViewModel<T> : BaseEditViewModel where T : class, new()
    {
        private readonly object _lockObj = new object();

        /// <summary>
        /// 增加数据项命令
        /// </summary>
        public MvxCommand AddItemCommand { get; private set; }

        /// <summary>
        /// 移除数据项命令
        /// </summary>
        public MvxCommand RemoveItemCommand { get; private set; }

        private ObservableCollection<T> _itemSource;

        /// <summary>
        /// 数据项集合
        /// </summary>
        public ObservableCollection<T> ItemSource
        {
            get => _itemSource;
            set
            {
                lock (_lockObj)
                {
                    if (_itemSource != value)
                    {
                        UnSubscribeItemSourceOnCollectionChanged(_itemSource);

                        _itemSource = value;

                        SubscribeItemSourceOnCollectionChanged(_itemSource);
                        CollectionCountChanged();

                        RaisePropertyChanged(nameof(ItemSource));
                    }
                }
            }
        }

        /// <summary>
        /// 选中项
        /// </summary>
        public virtual T SelectedItem { get; set; }

        /// <summary>
        /// 是否全选
        /// </summary>
        public bool IsCheckedAll { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        protected BaseEditListViewModel()
        {
            SetIsSupportChecked(typeof(T));

            AddItemCommand = new MvxCommand(AddItemCommandHandler);
            RemoveItemCommand = new MvxCommand(RemoveItemCommandHandler);
        }

        /// <summary>
        /// 增加数据处理
        /// </summary>
        protected virtual void AddItemCommandHandler()
        {
            lock (_lockObj)
            {
                if (ItemSource == null)
                {
                    ItemSource = new ObservableCollection<T>();
                }

                var newItem = new T();
                ItemSource.Add(newItem);

                SelectedItem = newItem;
            }
        }

        /// <summary>
        /// 移除数据处理
        /// </summary>
        protected virtual void RemoveItemCommandHandler()
        {
            lock (_lockObj)
            {
                if (SelectedItem != null)
                {
                    ItemSource?.Remove(SelectedItem);
                }
            }
        }

        /// <summary>
        /// 集合数据项变更
        /// </summary>
        protected virtual void CollectionCountChanged()
        {
        }

        #region IsChecked相关

        /// <summary>
        /// 由数据项是否选中触发的全选是否选中
        /// </summary>
        private bool _checkedAllChangedFromItems;

        /// <summary>
        /// 全选状态变更由自身控制
        /// </summary>
        private bool _checkedAllChangedFromSelf;

        /// <summary>
        /// 数据项是否支持选中
        /// </summary>
        private bool _supportChecked;

        /// <summary>
        /// Model是否继承ISupportChecked接口
        /// </summary>
        private bool _modelIsAssignableFromISupportChecked;

        /// <summary>
        /// 是否全部选中变更
        /// </summary>
        private void OnIsCheckedAllChanged()
        {
            if (_checkedAllChangedFromItems)
            {
                return;
            }

            if (ItemSource != null)
            {
                _checkedAllChangedFromSelf = true;

                List<T> list;
                lock (_lockObj)
                {
                    list = ItemSource.ToList();
                }

                if (IsCheckedAll)
                {
                    foreach (var item in list)
                    {
                        SetIsChecked(item, true);
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        SetIsChecked(item, false);
                    }
                }

                _checkedAllChangedFromSelf = false;
            }
        }

        /// <summary>
        /// 订阅数据项集合的变更
        /// </summary>
        private void SubscribeItemSourceOnCollectionChanged(ObservableCollection<T> collection)
        {
            lock (_lockObj)
            {
                if (collection != null)
                {
                    collection.CollectionChanged += ItemSourceOnCollectionChanged;

                    if (!_supportChecked)
                    {
                        return;
                    }

                    foreach (var item in collection)
                    {
                        SubscribeItemIsChecked(item);
                    }
                }
            }
        }

        /// <summary>
        /// 移除订阅数据项集合的变更
        /// </summary>
        private void UnSubscribeItemSourceOnCollectionChanged(ObservableCollection<T> collection)
        {
            lock (_lockObj)
            {
                if (collection != null)
                {
                    collection.CollectionChanged -= ItemSourceOnCollectionChanged;

                    if (!_supportChecked)
                    {
                        return;
                    }

                    foreach (var item in collection)
                    {
                        UnSubscribeItemIsChecked(item);
                    }
                }
            }
        }

        /// <summary>
        /// 集合数据项变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionCountChanged();

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (sender is IEnumerable list)
                {
                    foreach (var i in list)
                    {
                        UnSubscribeItemIsChecked(i as T);
                    }
                }
                return;
            }

            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    UnSubscribeItemIsChecked(oldItem as T);
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    SubscribeItemIsChecked(newItem as T);
                }
            }
        }

        /// <summary>
        /// 设置是否支持复选选中
        /// </summary>
        private void SetIsSupportChecked(Type modelType)
        {
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(modelType))
            {
                if (typeof(ISupportChecked).IsAssignableFrom(modelType))
                {
                    _modelIsAssignableFromISupportChecked = true;
                    _supportChecked = true;
                }
                else
                {
                    //判断有没有相关属性名的属性
                    var property = modelType.GetProperty(nameof(ISupportChecked.IsChecked),
                        BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        _supportChecked = true;
                    }
                }
            }
        }

        private bool? GetIsChecked(T model)
        {
            if (_supportChecked && model != null)
            {
                if (_modelIsAssignableFromISupportChecked)
                {
                    return ((ISupportChecked)model).IsChecked;
                }
                else
                {
                    var property = typeof(T).GetProperty(nameof(ISupportChecked.IsChecked),
                        BindingFlags.Public | BindingFlags.Instance);

                    if (property != null)
                    {
                        return (bool)property.GetValue(model);
                    }
                }
            }

            return null;
        }

        private void SetIsChecked(T model, bool value)
        {
            if (_supportChecked && model != null)
            {
                if (_modelIsAssignableFromISupportChecked)
                {
                    ((ISupportChecked)model).IsChecked = value;
                }
                else
                {
                    var property = typeof(T).GetProperty(nameof(ISupportChecked.IsChecked),
                        BindingFlags.Public | BindingFlags.Instance);

                    if (property != null)
                    {
                        property.SetValue(model, value);
                    }
                }
            }
        }

        private void SubscribeItemIsChecked(T model)
        {
            if (model is INotifyPropertyChanged notifyPropertyChangedModel)
            {
                notifyPropertyChangedModel.PropertyChanged += NotifyPropertyChangedModelOnPropertyChanged;
            }
        }

        private void UnSubscribeItemIsChecked(T model)
        {
            if (model is INotifyPropertyChanged notifyPropertyChangedModel)
            {
                notifyPropertyChangedModel.PropertyChanged -= NotifyPropertyChangedModelOnPropertyChanged;
            }
        }

        private void NotifyPropertyChangedModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_checkedAllChangedFromSelf)
            {
                return;
            }

            if (e.PropertyName == nameof(ISupportChecked.IsChecked))
            {
                List<T> list;
                lock (_lockObj)
                {
                    list = ItemSource.ToList();
                }

                var isAnyNotChecked = list.Any(s =>
                {
                    var isChecked = GetIsChecked(s);
                    return isChecked.HasValue && !isChecked.Value || !isChecked.HasValue;
                });

                _checkedAllChangedFromItems = true;
                IsCheckedAll = !isAnyNotChecked;
                _checkedAllChangedFromItems = false;
            }
        }

        #endregion
    }

    /// <summary>
    /// 带列表的基础ViewModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParameter"></typeparam>
    public abstract class BaseEditListViewModel<T, TParameter> : BaseEditViewModel<TParameter> where T : class, new()
    {
        private readonly object _lockObj = new object();

        /// <summary>
        /// 增加数据项命令
        /// </summary>
        public MvxCommand AddItemCommand { get; private set; }

        /// <summary>
        /// 移除数据项命令
        /// </summary>
        public MvxCommand RemoveItemCommand { get; private set; }

        private ObservableCollection<T> _itemSource;

        /// <summary>
        /// 数据项集合
        /// </summary>
        public ObservableCollection<T> ItemSource
        {
            get => _itemSource;
            set
            {
                lock (_lockObj)
                {
                    if (_itemSource != value)
                    {
                        UnSubscribeItemSourceOnCollectionChanged(_itemSource);

                        _itemSource = value;

                        SubscribeItemSourceOnCollectionChanged(_itemSource);
                        CollectionCountChanged();

                        RaisePropertyChanged(nameof(ItemSource));
                    }
                }
            }
        }

        /// <summary>
        /// 选中项
        /// </summary>
        public virtual T SelectedItem { get; set; }

        /// <summary>
        /// 是否全选
        /// </summary>
        public bool IsCheckedAll { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        protected BaseEditListViewModel()
        {
            SetIsSupportChecked(typeof(T));

            AddItemCommand = new MvxCommand(AddItemCommandHandler);
            RemoveItemCommand = new MvxCommand(RemoveItemCommandHandler);
        }

        /// <summary>
        /// 增加数据处理
        /// </summary>
        protected virtual void AddItemCommandHandler()
        {
            lock (_lockObj)
            {
                if (ItemSource == null)
                {
                    ItemSource = new ObservableCollection<T>();
                }

                var newItem = new T();
                ItemSource.Add(newItem);

                SelectedItem = newItem;
            }
        }

        /// <summary>
        /// 移除数据处理
        /// </summary>
        protected virtual void RemoveItemCommandHandler()
        {
            lock (_lockObj)
            {
                if (SelectedItem != null)
                {
                    ItemSource?.Remove(SelectedItem);
                }
            }
        }

        /// <summary>
        /// 集合数据项变更
        /// </summary>
        protected virtual void CollectionCountChanged()
        {
        }

        #region IsChecked相关

        /// <summary>
        /// 由数据项是否选中触发的全选是否选中
        /// </summary>
        private bool _checkedAllChangedFromItems;

        /// <summary>
        /// 全选状态变更由自身控制
        /// </summary>
        private bool _checkedAllChangedFromSelf;

        /// <summary>
        /// 数据项是否支持选中
        /// </summary>
        private bool _supportChecked;

        /// <summary>
        /// Model是否继承ISupportChecked接口
        /// </summary>
        private bool _modelIsAssignableFromISupportChecked;

        /// <summary>
        /// 是否全部选中变更
        /// </summary>
        private void OnIsCheckedAllChanged()
        {
            if (_checkedAllChangedFromItems)
            {
                return;
            }

            if (ItemSource != null)
            {
                _checkedAllChangedFromSelf = true;

                List<T> list;
                lock (_lockObj)
                {
                    list = ItemSource.ToList();
                }

                if (IsCheckedAll)
                {
                    foreach (var item in list)
                    {
                        SetIsChecked(item, true);
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        SetIsChecked(item, false);
                    }
                }

                _checkedAllChangedFromSelf = false;
            }
        }

        /// <summary>
        /// 订阅数据项集合的变更
        /// </summary>
        private void SubscribeItemSourceOnCollectionChanged(ObservableCollection<T> collection)
        {
            lock (_lockObj)
            {
                if (collection != null)
                {
                    collection.CollectionChanged += ItemSourceOnCollectionChanged;

                    if (!_supportChecked)
                    {
                        return;
                    }

                    foreach (var item in collection)
                    {
                        SubscribeItemIsChecked(item);
                    }
                }
            }
        }

        /// <summary>
        /// 移除订阅数据项集合的变更
        /// </summary>
        private void UnSubscribeItemSourceOnCollectionChanged(ObservableCollection<T> collection)
        {
            lock (_lockObj)
            {
                if (collection != null)
                {
                    collection.CollectionChanged -= ItemSourceOnCollectionChanged;

                    if (!_supportChecked)
                    {
                        return;
                    }

                    foreach (var item in collection)
                    {
                        UnSubscribeItemIsChecked(item);
                    }
                }
            }
        }

        /// <summary>
        /// 集合数据项变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionCountChanged();

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (sender is IEnumerable list)
                {
                    foreach (var i in list)
                    {
                        UnSubscribeItemIsChecked(i as T);
                    }
                }
                return;
            }

            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    UnSubscribeItemIsChecked(oldItem as T);
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    SubscribeItemIsChecked(newItem as T);
                }
            }
        }

        /// <summary>
        /// 设置是否支持复选选中
        /// </summary>
        private void SetIsSupportChecked(Type modelType)
        {
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(modelType))
            {
                if (typeof(ISupportChecked).IsAssignableFrom(modelType))
                {
                    _modelIsAssignableFromISupportChecked = true;
                    _supportChecked = true;
                }
                else
                {
                    //判断有没有相关属性名的属性
                    var property = modelType.GetProperty(nameof(ISupportChecked.IsChecked),
                        BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        _supportChecked = true;
                    }
                }
            }
        }

        private bool? GetIsChecked(T model)
        {
            if (_supportChecked && model != null)
            {
                if (_modelIsAssignableFromISupportChecked)
                {
                    return ((ISupportChecked)model).IsChecked;
                }
                else
                {
                    var property = typeof(T).GetProperty(nameof(ISupportChecked.IsChecked),
                        BindingFlags.Public | BindingFlags.Instance);

                    if (property != null)
                    {
                        return (bool)property.GetValue(model);
                    }
                }
            }

            return null;
        }

        private void SetIsChecked(T model, bool value)
        {
            if (_supportChecked && model != null)
            {
                if (_modelIsAssignableFromISupportChecked)
                {
                    ((ISupportChecked)model).IsChecked = value;
                }
                else
                {
                    var property = typeof(T).GetProperty(nameof(ISupportChecked.IsChecked),
                        BindingFlags.Public | BindingFlags.Instance);

                    if (property != null)
                    {
                        property.SetValue(model, value);
                    }
                }
            }
        }

        private void SubscribeItemIsChecked(T model)
        {
            if (model is INotifyPropertyChanged notifyPropertyChangedModel)
            {
                notifyPropertyChangedModel.PropertyChanged += NotifyPropertyChangedModelOnPropertyChanged;
            }
        }

        private void UnSubscribeItemIsChecked(T model)
        {
            if (model is INotifyPropertyChanged notifyPropertyChangedModel)
            {
                notifyPropertyChangedModel.PropertyChanged -= NotifyPropertyChangedModelOnPropertyChanged;
            }
        }

        private void NotifyPropertyChangedModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_checkedAllChangedFromSelf)
            {
                return;
            }

            if (e.PropertyName == nameof(ISupportChecked.IsChecked))
            {
                List<T> list;
                lock (_lockObj)
                {
                    list = ItemSource.ToList();
                }

                var isAnyNotChecked = list.Any(s =>
                {
                    var isChecked = GetIsChecked(s);
                    return isChecked.HasValue && !isChecked.Value || !isChecked.HasValue;
                });

                _checkedAllChangedFromItems = true;
                IsCheckedAll = !isAnyNotChecked;
                _checkedAllChangedFromItems = false;
            }
        }

        #endregion
    }
}
