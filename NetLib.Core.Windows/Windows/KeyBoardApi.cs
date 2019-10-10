using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public class KeyBoardApi
    {
        internal KeyBoardApi()
        {
        }

        private const byte KeyDownFlag = 0;
        private const byte KeyUpFlag = 2;

        /// <summary>
        /// 鼠标按压持续时间
        /// </summary>
        private const uint KeyPressedTime = 800;

        /// <summary>
        /// keybd_event
        /// </summary>
        /// <param name="bVk">按键的虚拟键值</param>
        /// <param name="bScan">扫描码，一般不用设置，用0代替就行</param>
        /// <param name="dwFlags">选项标志：0：表示按下，2：表示松开</param>
        /// <param name="dwExtraInfo">一般设置为0</param>
        [DllImport("user32.dll")]
        // ReSharper disable once IdentifierTypo
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        /// <summary>
        /// KeyBoard click
        /// </summary>
        /// <param name="key">key name</param>
        public void KeyClick(Key key)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            var keyByte = (byte) KeyInterop.VirtualKeyFromKey(key);
            keybd_event(keyByte, 0, KeyDownFlag, IntPtr.Zero);
            keybd_event(keyByte, 0, KeyUpFlag, IntPtr.Zero);

            WindowsApi.WriteLog($"{nameof(KeyClick)} {key}");
        }

        /// <summary>
        /// KeyBoard click
        /// </summary>
        /// <param name="keys">key names</param>
        public void KeyClick(params Key[] keys)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var key in keys)
            {
                keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyDownFlag, IntPtr.Zero);
            }

            foreach (var key in keys)
            {
                keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyUpFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyClick)} {key}");
            }
        }

        /// <summary>
        /// KeyBoard down
        /// </summary>
        /// <param name="key">key name</param>
        public void KeyDown(Key key)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyDownFlag, IntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(KeyDown)} {key}");
        }

        /// <summary>
        /// KeyBoard down
        /// </summary>
        /// <param name="keys">key names</param>
        public void KeyDown(params Key[] keys)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var key in keys)
            {
                keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyDownFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyDown)} {key}");
            }
        }

        /// <summary>
        /// KeyBoard up
        /// </summary>
        /// <param name="key">key name</param>
        public void KeyUp(Key key)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyUpFlag, IntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(KeyUp)} {key}");
        }

        /// <summary>
        /// KeyBoard up
        /// </summary>
        /// <param name="keys">key names</param>
        public void KeyUp(params Key[] keys)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var key in keys)
            {
                keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyUpFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyUp)} {key}");
            }
        }

        /// <summary>
        /// KeyPress
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="pressedMillionSeconds">pressed time</param>
        /// <returns></returns>
        public static async Task KeyPress(Key key, uint pressedMillionSeconds = KeyPressedTime)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            var keyByte = (byte) KeyInterop.VirtualKeyFromKey(key);
            keybd_event(keyByte, 0, KeyDownFlag, IntPtr.Zero);

            if (pressedMillionSeconds != 0u)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(pressedMillionSeconds));
            }

            keybd_event(keyByte, 0, KeyUpFlag, IntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(KeyPress)} {key} {nameof(KeyPressedTime)}:{pressedMillionSeconds}");
        }

        /// <summary>
        /// KeyPress
        /// </summary>
        /// <param name="pressedMillionSeconds">pressed time</param>
        /// <param name="keys">key names</param>
        /// <returns></returns>
        public static async Task KeyPress(uint pressedMillionSeconds = KeyPressedTime, params Key[] keys)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            foreach (var key in keys)
            {
                keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyDownFlag, IntPtr.Zero);
            }

            if (pressedMillionSeconds != 0u)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(pressedMillionSeconds));
            }

            foreach (var key in keys)
            {
                keybd_event((byte) KeyInterop.VirtualKeyFromKey(key), 0, KeyUpFlag, IntPtr.Zero);
                WindowsApi.WriteLog($"{nameof(KeyPress)} {key} {nameof(KeyPressedTime)}:{pressedMillionSeconds}");
            }
        }

        /// <summary>
        /// Input text
        /// </summary>
        /// <param name="text">text</param>
        public void InputString(string text)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            if (text != null)
            {
                SendKeys.SendWait(text);
                WindowsApi.WriteLog($"{nameof(InputString)} {nameof(text)} is \"{text}\".");
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(InputString)} {nameof(text)} is null.");
            }
        }
    }
}
