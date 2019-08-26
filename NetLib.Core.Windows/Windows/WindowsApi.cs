using System;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public static class WindowsApi
    {
        /// <summary>
        /// 接受新的WindowsApi操作日志
        /// </summary>
        public static event EventHandler<string> ReceiveApiOperateLogEvent;

        /// <summary>
        /// 记录日志时间
        /// </summary>
        public static bool NeedLogTime { get; set; } = true;

        /// <summary>
        /// 操作延迟，减慢操作间的步骤，毫秒ms（在操作前停顿的时长）
        /// </summary>
        public static int? Delay { get; set; }

        /// <summary>
        /// MouseApi
        /// </summary>
        public static MouseApi MouseApi { get; } = new MouseApi();

        /// <summary>
        /// KeyBoardApi
        /// </summary>
        public static KeyBoardApi KeyBoardApi { get; } = new KeyBoardApi();

        /// <summary>
        /// WindowApi
        /// </summary>
        public static WindowApi WindowApi { get; } = new WindowApi();

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log">日志信息</param>
        internal static void WriteLog(string log)
        {
            if (ReceiveApiOperateLogEvent != null)
            {
                if (NeedLogTime)
                {
                    log = $"{DateTime.Now:yyyy-MM-dd hh:mm:ss:ffff}  {log}";
                }

                ReceiveApiOperateLogEvent.Invoke(null, log);
            }
        }
    }
}
