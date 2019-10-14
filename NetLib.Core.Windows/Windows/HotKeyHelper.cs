using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public class HotKeyHelper
    {
        internal HotKeyHelper()
        {
            HotkeyManager.HotkeyAlreadyRegistered += (sender, args) => HotkeyAlreadyRegistered?.Invoke(sender, args);
        }

        /// <summary>
        /// 带有Identity的Cache，可以被移除
        /// Key:Identity
        /// </summary>
        private readonly Dictionary<string, HotKeyIdentity> _identityCache = new Dictionary<string, HotKeyIdentity>();

        /// <summary>
        /// 带有全部快捷键的Cache
        /// </summary>
        private readonly Dictionary<HotKeyIdentity, HotKeyModel> _hotKeyCache =
            new Dictionary<HotKeyIdentity, HotKeyModel>();

        private readonly object _lock = new object();

        /// <summary>
        /// 热键已注册事件
        /// </summary>
        public event EventHandler<HotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifierKeys"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool Register(Key key, ModifierKeys modifierKeys, Action action)
        {
            return RegisterOrReplace(string.Empty, key, modifierKeys, action);
        }

        /// <summary>
        /// 注册或替换快捷键
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="key"></param>
        /// <param name="modifierKeys"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RegisterOrReplace(string identity, Key key, ModifierKeys modifierKeys, Action action)
        {
            lock (_lock)
            {
                try
                {
                    Remove(identity);

                    var hotKeyIdentity = new HotKeyIdentity(key, modifierKeys);

                    if (_hotKeyCache.ContainsKey(hotKeyIdentity))
                    {
                        _hotKeyCache[hotKeyIdentity].LinkedEventHandler(action, identity);
                    }
                    else
                    {
                        var hotKeyModel = new HotKeyModel(hotKeyIdentity);
                        hotKeyModel.RemoveAllLinkedEvent += HotKeyModelOnRemoveAllLinkedEvent;
                        hotKeyModel.LinkedEventHandler(action, identity);

                        _hotKeyCache.Add(hotKeyIdentity, hotKeyModel);
                    }

                    if (!string.IsNullOrEmpty(identity))
                    {
                        _identityCache.Add(identity, hotKeyIdentity);
                    }

                    return true;
                }
                catch (HotkeyAlreadyRegisteredException hotkeyAlreadyRegisteredException)
                {
                    WindowsApi.WriteLog($"Hot key {hotkeyAlreadyRegisteredException.Name} already register");
                    HotkeyAlreadyRegistered?.Invoke(HotkeyManager.Current,
                        new HotkeyAlreadyRegisteredEventArgs(hotkeyAlreadyRegisteredException.Name));
                }
            }

            return false;
        }

        /// <summary>
        /// 移除快捷键的所有关联事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotKeyModelOnRemoveAllLinkedEvent(object sender, EventArgs e)
        {
            lock (_lock)
            {
                var hotKeyModel = (HotKeyModel) sender;
                hotKeyModel.RemoveAllLinkedEvent -= HotKeyModelOnRemoveAllLinkedEvent;

                if (_hotKeyCache.ContainsKey(hotKeyModel.HotKeyIdentity))
                {
                    _hotKeyCache.Remove(hotKeyModel.HotKeyIdentity);
                }

                var f = _identityCache.FirstOrDefault(s => s.Value == hotKeyModel.HotKeyIdentity);
                if (!string.IsNullOrEmpty(f.Key))
                {
                    _identityCache.Remove(f.Key);
                }
            }
        }

        /// <summary>
        /// 移除快捷键注册
        /// </summary>
        /// <param name="identity"></param>
        public void Remove(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return;
            }

            lock (_lock)
            {
                if (_identityCache.ContainsKey(identity))
                {
                    var hotkeyIdentity = _identityCache[identity];
                    _identityCache.Remove(identity);

                    if (_hotKeyCache.ContainsKey(hotkeyIdentity))
                    {
                        _hotKeyCache[hotkeyIdentity].TryRemoveEventHandler(identity);
                    }
                }
            }
        }

        private struct HotKeyIdentity : IEquatable<HotKeyIdentity>
        {
            public Key Key { get; }

            public ModifierKeys ModifierKeys { get; }

            public string RegisterKey { get; }

            public HotKeyIdentity(Key key, ModifierKeys modifierKeys)
            {
                Key = key;
                ModifierKeys = modifierKeys;

                var keyString = new StringBuilder();

                foreach (ModifierKeys value in Enum.GetValues(typeof(ModifierKeys)))
                {
                    if (modifierKeys.HasFlag(value))
                    {
                        keyString.Append(value);
                    }
                }

                keyString.Append($"_{key}");
                RegisterKey = keyString.ToString();
            }

            public bool Equals(HotKeyIdentity other)
            {
                return Key == other.Key && ModifierKeys == other.ModifierKeys;
            }

            public override bool Equals(object obj)
            {
                return obj is HotKeyIdentity other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((int) Key * 397) ^ (int) ModifierKeys;
                }
            }

            public static bool operator ==(HotKeyIdentity a, HotKeyIdentity b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(HotKeyIdentity a, HotKeyIdentity b)
            {
                return !a.Equals(b);
            }
        }

        private class HotKeyModel
        {
            /// <summary>
            /// HotKeyIdentity
            /// </summary>
            public HotKeyIdentity HotKeyIdentity { get; }

            /// <summary>
            /// 带Identity的链接的子事件
            /// </summary>
            private Dictionary<string, WeakReferenceDelegate> _linkedEventHandlersWithIdentity;

            /// <summary>
            /// 所有链接的子事件
            /// </summary>
            private List<WeakReferenceDelegate> _linkedEventHandlers;

            /// <summary>
            /// 移除所有链接的事件
            /// </summary>
            public event EventHandler RemoveAllLinkedEvent;

            public HotKeyModel(HotKeyIdentity hotKeyIdentity)
            {
                HotKeyIdentity = hotKeyIdentity;

                //构造时尝试注册

                HotkeyManager.Current.AddOrReplace(HotKeyIdentity.RegisterKey, HotKeyIdentity.Key,
                    HotKeyIdentity.ModifierKeys, OnHotkeyEventHandler);
            }

            /// <summary>
            /// 链接快捷键处理事件
            /// </summary>
            /// <param name="action">动作</param>
            /// <param name="identity">标识</param>
            public void LinkedEventHandler(Action action, string identity = "")
            {
                if (_linkedEventHandlers == null)
                {
                    // ReSharper disable once InconsistentlySynchronizedField
                    _linkedEventHandlers = new List<WeakReferenceDelegate>();
                }

                lock (_linkedEventHandlers)
                {
                    _linkedEventHandlers.Add(new WeakReferenceDelegate(action.Target, action));
                }

                if (!string.IsNullOrEmpty(identity))
                {
                    if (_linkedEventHandlersWithIdentity == null)
                    {
                        // ReSharper disable once InconsistentlySynchronizedField
                        _linkedEventHandlersWithIdentity = new Dictionary<string, WeakReferenceDelegate>();
                    }

                    lock (_linkedEventHandlersWithIdentity)
                    {
                        _linkedEventHandlersWithIdentity.Add(identity,
                            new WeakReferenceDelegate(action.Target, action));
                    }
                }
            }

            /// <summary>
            /// 尝试移除事件
            /// </summary>
            /// <param name="identity"></param>
            public void TryRemoveEventHandler(string identity)
            {
                if (!string.IsNullOrEmpty(identity))
                {
                    if (_linkedEventHandlersWithIdentity != null &&
                        // ReSharper disable once InconsistentlySynchronizedField
                        _linkedEventHandlersWithIdentity.ContainsKey(identity))
                    {
                        // ReSharper disable once InconsistentlySynchronizedField
                        var action = _linkedEventHandlersWithIdentity[identity];

                        lock (_linkedEventHandlersWithIdentity)
                        {
                            _linkedEventHandlersWithIdentity.Remove(identity);
                        }

                        if (_linkedEventHandlers != null)
                        {
                            bool isRemoveAll;
                            lock (_linkedEventHandlers)
                            {
                                _linkedEventHandlers.Remove(action);
                                isRemoveAll = !_linkedEventHandlers.Any();
                            }

                            if (isRemoveAll)
                            {
                                //移除快捷键处理事件
                                HotkeyManager.Current.Remove(HotKeyIdentity.RegisterKey);
                                RemoveAllLinkedEvent?.Invoke(this, EventArgs.Empty);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 处理事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="args"></param>
            private void OnHotkeyEventHandler(object sender, HotkeyEventArgs args)
            {
                if (_linkedEventHandlers != null)
                {
                    lock (_linkedEventHandlers)
                    {
                        if (_linkedEventHandlers.RemoveAll(s => !s.IsAlive()) > 0 &&
                            _linkedEventHandlersWithIdentity != null)
                        {
                            var removeIdentity = new List<string>();

                            foreach (var keyValuePair in _linkedEventHandlersWithIdentity)
                            {
                                if (!keyValuePair.Value.IsAlive())
                                {
                                    removeIdentity.Add(keyValuePair.Key);
                                }
                            }

                            foreach (var identity in removeIdentity)
                            {
                                _linkedEventHandlersWithIdentity.Remove(identity);
                            }
                        }

                        foreach (var linkedEventHandler in _linkedEventHandlers)
                        {
                            try
                            {
                                linkedEventHandler.Trigger();
                            }
                            catch (Exception e)
                            {
                                WindowsApi.WriteLog(e.Message);
                                Debug.WriteLine(e);
                            }
                        }
                    }
                }
            }

            private class WeakReferenceDelegate
            {
                private readonly WeakReference _weakReference;
                private readonly MethodInfo _methodInfo;

                public WeakReferenceDelegate(object target, Action action)
                {
                    _methodInfo = action.Method;

                    if (!_methodInfo.IsStatic)
                    {
                        _weakReference = new WeakReference(target);
                    }
                }

                public bool IsAlive()
                {
                    return _methodInfo.IsStatic || _weakReference.IsAlive;
                }

                public void Trigger()
                {
                    if (_methodInfo.IsStatic)
                    {
                        _methodInfo.Invoke(null, null);
                    }
                    else if (_weakReference.IsAlive)
                    {
                        _methodInfo.Invoke(_weakReference.Target, null);
                    }
                }
            }
        }
    }
}