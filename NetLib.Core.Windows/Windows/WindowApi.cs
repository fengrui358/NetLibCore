using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public class WindowApi
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        internal WindowApi()
        {
        }

        #region Obsolete

        /*

        /// <summary>
        /// Activate the process if it not minimize.
        /// </summary>
        /// <param name="process">process</param>
        /// <returns>success</returns>
        public bool SetForegroundWindow(Process process)
        {
            var result = false;

            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            if (process != null)
            {
                result = SetForegroundWindow(process.MainWindowHandle);
                WindowsApi.WriteLog(
                    $"{nameof(SetForegroundWindow)} {nameof(process)} name is {process.ProcessName}, main window handle is {process.MainWindowHandle}, {nameof(result)} is {result}.");
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(SetForegroundWindow)} {nameof(process)} is null.");
            }

            return result;
        }

        /// <summary>
        /// Activate the form using the form handle if it not minimize.
        /// </summary>
        /// <param name="hWnd">form handle</param>
        /// <returns>success</returns>
        public bool SetForegroundWindowWithHandle(IntPtr hWnd)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            var result = SetForegroundWindow(hWnd);
            WindowsApi.WriteLog($"{nameof(SetForegroundWindow)} handle is {hWnd}, {nameof(result)} is {result}.");

            return result;
        }

        */

        #endregion

        /// <summary>
        /// Activate the process.
        /// </summary>
        /// <param name="process"></param>
        public void SwitchToThisWindow(Process process)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            if (process != null)
            {
                SwitchToThisWindow(process.MainWindowHandle, true);
                WindowsApi.WriteLog(
                    $"{nameof(SwitchToThisWindow)} {nameof(process)} name is {process.ProcessName}, main window handle is {process.MainWindowHandle}.");
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(SwitchToThisWindow)} {nameof(process)} is null.");
            }
        }

        /// <summary>
        /// Activate the form using the form handle.
        /// </summary>
        /// <param name="hWnd">form handle</param>
        public void SwitchToThisWindow(IntPtr hWnd)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            //激活显示在最前面
            SwitchToThisWindow(hWnd, true);
            WindowsApi.WriteLog($"{nameof(SwitchToThisWindow)} handle is {hWnd}.");
        }
    }
}
