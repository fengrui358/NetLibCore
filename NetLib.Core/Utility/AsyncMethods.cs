using System;
using System.Collections.Generic;
using System.Threading;

namespace FrHello.NetLib.Core.Utility
{
    /// <summary>
    /// 异步辅助方法
    /// </summary>
    public static class AsyncMethods
    {
        private static readonly Dictionary<Action, DeBounce> DeBounces = new Dictionary<Action, DeBounce>();
        private static readonly Dictionary<Action, Throttle> Throttles = new Dictionary<Action, Throttle>();

        /// <summary>
        /// 防抖调用函数
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="interval">interval, ms</param>
        /// <param name="leading">immediate execute</param>
        public static DeBounce DeBounce(Action action, int interval, bool leading = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (interval < 0)
            {
                throw new ArgumentException(nameof(interval));
            }

            if (DeBounces.ContainsKey(action))
            {
                DeBounces[action].Update();

                return DeBounces[action];
            }
            else
            {
                var deBounce = new DeBounce(action, interval, leading);
                deBounce.Finished += (sender, args) =>
                {
                    if (deBounce.Action != null && DeBounces.ContainsKey(deBounce.Action))
                    {
                        DeBounces.Remove(deBounce.Action);
                    }
                };

                DeBounces.Add(action, deBounce);

                return deBounce;
            }
        }

        /// <summary>
        /// 节流调用函数
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="interval">interval, ms</param>
        /// <param name="leading">immediate execute</param>
        public static Throttle Throttle(Action action, int interval, bool leading = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (interval < 0)
            {
                throw new ArgumentException(nameof(interval));
            }

            if (Throttles.ContainsKey(action))
            {
                Throttles[action].Update();

                return Throttles[action];
            }
            else
            {
                var throttle = new Throttle(action, interval, leading);
                throttle.Finished += (sender, args) =>
                {
                    if (throttle.Action != null && Throttles.ContainsKey(throttle.Action))
                    {
                        Throttles.Remove(throttle.Action);
                    }
                };

                Throttles.Add(action, throttle);

                return throttle;
            }
        }
    }

    /// <summary>
    /// 防抖
    /// </summary>
    public class DeBounce
    {
        /// <summary>
        /// 时间到达时需要执行方法
        /// </summary>
        private bool _needInvoke;

        /// <summary>
        /// 需要执行的方法
        /// </summary>
        internal Action Action;

        /// <summary>
        /// 间隔
        /// </summary>
        private int _interval;

        private Timer _timer;

        /// <summary>
        /// DeBounce
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <param name="leading"></param>
        internal DeBounce(Action action, int interval, bool leading = false)
        {
            Action = action;
            _interval = interval;

            _needInvoke = true;

            _timer = new Timer(InnerInvoke, null, leading ? 0 : _interval, Timeout.Infinite);
        }

        /// <summary>
        /// 取消执行
        /// </summary>
        public void Cancel()
        {
            if (Action == null) return;

            Finished?.Invoke(this, EventArgs.Empty);

            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _timer = null;
            Action = null;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="interval">间隔</param>
        internal void Update(int? interval = null)
        {
            if (interval != null)
            {
                if (interval < 0)
                {
                    throw new ArgumentException(nameof(interval));
                }

                _interval = interval.Value;
            }

            _needInvoke = true;
            _timer?.Change(_interval, Timeout.Infinite);
        }

        /// <summary>
        /// 结束调用
        /// </summary>
        internal event EventHandler Finished;

        /// <summary>
        /// 时间到达时执行
        /// </summary>
        /// <param name="obj"></param>
        private void InnerInvoke(object obj)
        {
            if (_needInvoke)
            {
                _needInvoke = false;
                _timer?.Change(_interval, Timeout.Infinite);

                try
                {
                    Action?.Invoke();
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                Cancel();
            }
        }
    }

    /// <summary>
    /// 节流
    /// </summary>
    public class Throttle
    {
        /// <summary>
        /// 时间到达时需要执行方法
        /// </summary>
        private bool _needInvoke;

        /// <summary>
        /// 需要执行的方法
        /// </summary>
        internal Action Action;

        /// <summary>
        /// 间隔
        /// </summary>
        private int _interval;

        private Timer _timer;

        /// <summary>
        /// DeBounce
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <param name="leading"></param>
        internal Throttle(Action action, int interval, bool leading = true)
        {
            Action = action;
            _interval = interval;

            _needInvoke = true;

            _timer = new Timer(InnerInvoke, null, leading ? 0 : _interval, Timeout.Infinite);
        }

        /// <summary>
        /// 取消执行
        /// </summary>
        public void Cancel()
        {
            if (Action == null) return;

            Finished?.Invoke(this, EventArgs.Empty);

            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _timer = null;
            Action = null;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="interval">间隔</param>
        internal void Update(int? interval = null)
        {
            if (interval != null)
            {
                if (interval < 0)
                {
                    throw new ArgumentException(nameof(interval));
                }

                _interval = interval.Value;
            }

            _needInvoke = true;
        }

        /// <summary>
        /// 结束调用
        /// </summary>
        internal event EventHandler Finished;

        /// <summary>
        /// 时间到达时执行
        /// </summary>
        /// <param name="obj"></param>
        private void InnerInvoke(object obj)
        {
            if (_needInvoke)
            {
                _needInvoke = false;
                _timer?.Change(_interval, Timeout.Infinite);

                try
                {
                    Action?.Invoke();
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                Cancel();
            }
        }
    }
}
