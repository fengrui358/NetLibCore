using System;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public static class WindowsApi
    {
        private static string _lastLogMsg;
        private static DateTime _lastLogDateTime;

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
        /// ScreenApi
        /// </summary>
        public static ScreenApi ScreenApi { get; } = new ScreenApi();

        /// <summary>
        /// 热键注册
        /// </summary>
        public static HotKeyHelper HotKeyHelper { get; } = new HotKeyHelper();

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log">日志信息</param>
        internal static void WriteLog(string log)
        {
            if (ReceiveApiOperateLogEvent != null)
            {
                //降低日志频率，如果与上一条发送的日志一样并且发送时间小于1秒，则不发送
                if (log == _lastLogMsg && DateTime.Now.Subtract(_lastLogDateTime) < TimeSpan.FromSeconds(1))
                {
                    return;
                }

                _lastLogMsg = log;
                _lastLogDateTime = DateTime.Now;

                if (NeedLogTime)
                {
                    log = $"{DateTime.Now:yyyy-MM-dd hh:mm:ss:ffff}  {log}";
                }

                ReceiveApiOperateLogEvent.Invoke(null, log);
            }
        }
    }
}
