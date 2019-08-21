using System;

// ReSharper disable once CheckNamespace
namespace FrHello.NetLib.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 时间显示格式化
        /// </summary>
        /// <param name="timeSpan">时间值</param>
        /// <returns>格式化后的字符串hh:mm:ss(mm:ss)</returns>
        public static string TimeSpanFormat(this TimeSpan timeSpan)
        {
            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.Hours:d2}:{timeSpan.Minutes:d2}:{timeSpan.Seconds:d2}";
            }

            return $"{timeSpan.Minutes:d2}:{timeSpan.Seconds:d2}";
        }

        
    }

    /// <summary>
    /// 时间显示格式化
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// 时间显示格式化
        /// </summary>
        /// <param name="seconds">用秒统计的时间总数</param>
        /// <returns>格式化后的字符串hh:mm:ss(mm:ss)</returns>
        public static string TimeSpanFormat(double seconds)
        {
            var timeSpan = new TimeSpan(0, 0, 0, (int)Math.Round(seconds));
            return timeSpan.TimeSpanFormat();
        }
    }
}