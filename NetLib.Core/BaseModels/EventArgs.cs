using System;

namespace FrHello.NetLib.Core.BaseModels
{
    /// <summary>
    /// 带参数值的EventArgs
    /// </summary>
    /// <typeparam name="T">参数值的类型</typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// 参数值
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="value"></param>
        public EventArgs(T value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// 带参数值的EventArgs
    /// </summary>
    /// <typeparam name="T">参数值的类型</typeparam>
    public class ChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 旧的参数值
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// 新的参数值
        /// </summary>
        public T NewValue { get; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="oldValue">原值</param>
        /// <param name="newValue">新值</param>
        public ChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
