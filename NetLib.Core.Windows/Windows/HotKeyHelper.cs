using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        }

        /// <summary>
        /// 带有Identity的Cache，可以被移除
        /// Key:Identity
        /// </summary>
        private static readonly Dictionary<string, HotKeyIdentity> IdentityCache = new Dictionary<string, HotKeyIdentity>();

        /// <summary>
        /// 带有全部快捷键的Cache
        /// </summary>
        private static readonly Dictionary<HotKeyIdentity, HotKeyModel> HotKeyCache = new Dictionary<HotKeyIdentity, HotKeyModel>();

        private static readonly object Lock = new object();

        /// <summary>
        /// 热键已注册事件
        /// </summary>
        public static event EventHandler<HotkeyAlreadyRegisteredEventArgs> HotkeyAlreadyRegistered;

        static HotKeyHelper()
        {
            HotkeyManager.HotkeyAlreadyRegistered += (sender, args) => HotkeyAlreadyRegistered?.Invoke(sender, args);
        }

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifierKeys"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Register(Key key, ModifierKeys modifierKeys, Action action)
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
        public static bool RegisterOrReplace(string identity, Key key, ModifierKeys modifierKeys, Action action)
        {
            lock (Lock)
            {
                try
                {
                    Remove(identity);

                    var hotKeyIdentity = new HotKeyIdentity(key, modifierKeys);

                    if (HotKeyCache.ContainsKey(hotKeyIdentity))
                    {
                        HotKeyCache[hotKeyIdentity].LinkedEventHandler(action, identity);
                        return true;
                    }
                    else
                    {
                        var hotKeyModel = new HotKeyModel(hotKeyIdentity);
                        hotKeyModel.RemoveAllLinkedEvent += HotKeyModelOnRemoveAllLinkedEvent;
                        hotKeyModel.LinkedEventHandler(action, identity);

                        HotKeyCache.Add(hotKeyIdentity, hotKeyModel);

                        if (!string.IsNullOrEmpty(identity))
                        {
                            IdentityCache.Add(identity, hotKeyIdentity);
                        }

                        return true;
                    }
                }
                catch (HotkeyAlreadyRegisteredException hotkeyAlreadyRegisteredException)
                {
                    HotkeyAlreadyRegistered?.Invoke(HotkeyManager.Current,
                        new HotkeyAlreadyRegisteredEventArgs(hotkeyAlreadyRegisteredException.Name));
                    throw;
                }
            }
        }

        /// <summary>
        /// 移除快捷键的所有关联事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HotKeyModelOnRemoveAllLinkedEvent(object sender, EventArgs e)
        {
            lock (Lock)
            {
                var hotKeyModel = (HotKeyModel)sender;
                hotKeyModel.RemoveAllLinkedEvent -= HotKeyModelOnRemoveAllLinkedEvent;

                if (HotKeyCache.ContainsKey(hotKeyModel.HotKeyIdentity))
                {
                    HotKeyCache.Remove(hotKeyModel.HotKeyIdentity);
                }
            }
        }

        /// <summary>
        /// 移除快捷键注册
        /// </summary>
        /// <param name="identity"></param>
        public static void Remove(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return;
            }

            lock (Lock)
            {
                if (IdentityCache.ContainsKey(identity))
                {
                    var hotkeyIdentity = IdentityCache[identity];
                    IdentityCache.Remove(identity);

                    if (HotKeyCache.ContainsKey(hotkeyIdentity))
                    {
                        HotKeyCache[hotkeyIdentity].TryRemoveEventHandler(identity);
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
                    return ((int)Key * 397) ^ (int)ModifierKeys;
                }
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
            private Dictionary<string, Action> _linkedEventHandlersWithIdentity;

            /// <summary>
            /// 所有链接的子事件
            /// </summary>
            private List<Action> _linkedEventHandlers;

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
                    _linkedEventHandlers = new List<Action>();
                }

                lock (_linkedEventHandlers)
                {
                    _linkedEventHandlers.Add(action);
                }

                if (!string.IsNullOrEmpty(identity))
                {
                    if (_linkedEventHandlersWithIdentity == null)
                    {
                        // ReSharper disable once InconsistentlySynchronizedField
                        _linkedEventHandlersWithIdentity = new Dictionary<string, Action>();
                    }

                    lock (_linkedEventHandlersWithIdentity)
                    {
                        _linkedEventHandlersWithIdentity.Add(identity, action);
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
                        foreach (var linkedEventHandler in _linkedEventHandlers)
                        {
                            try
                            {
                                linkedEventHandler?.Invoke();
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
        }
    }
}
