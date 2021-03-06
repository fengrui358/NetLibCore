﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FrHello.NetLib.Core.Windows
{
    /// <summary>
    /// 运行进程判断工具
    /// </summary>
    public class RunningProcessHelper
    {
        #region 窗体打开模式

        private const int Hide = 0;
        private const int Normal = 1;
        private const int Maximize = 3;
        private const int ShowNoActivate = 4;
        private const int Show = 5;
        private const int Minimize = 6;
        private const int Restore = 9;
        private const int Showdefault = 10;

        #endregion

        /// <summary>
        /// 处理已经存在的窗口实例
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static Process RunningInstance(Process current)
        {
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    return process;
                }
            }
            return null;
        }

        /// <summary>
        /// 将已运行实例切换到显示状态
        /// </summary>
        /// <param name="instance"></param>
        public static void ActiveRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, ShowNoActivate);
            SetForegroundWindow(instance.MainWindowHandle);
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
