using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace FrHello.NetLib.Core.Windows.Windows
{
    internal class MouseApi
    {
        [Flags]
        internal enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            //XDown = 0x0080,
            //XUp = 0x0100,
            Wheel = 0x0800,
            //VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        private struct InnerPoint
        {
            int X { get; }
            int Y { get; }
            public InnerPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        //[DllImport("user32.dll")]
        //static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// 控制鼠标的Win32 Api
        /// </summary>
        /// <param name="flags">标志位集，指定点击按钮和鼠标动作的多种情况。</param>
        /// <remarks>
        /// 此参数里的各位可以是下列值的任何合理组合：
        /// Move：表明发生移动。
        /// LeftDown：表明接按下鼠标左键。
        /// LeftUp：表明松开鼠标左键。
        /// RightDown：表明按下鼠标右键。
        /// RightUp：表明松开鼠标右键。
        /// MiddleDown：表明按下鼠标中键。
        /// MiddleUp：表明松开鼠标中键。
        /// Wheel：在Windows NT中如果鼠标有一个轮，表明鼠标轮被移动。移动的数量由dwData给出。
        /// </remarks>
        /// <param name="dx">指定鼠标沿x轴的绝对位置或者从上次鼠标事件产生以来移动的数量，依赖于MouseEventFlag.Absolute的设置。给出的绝对数据作为鼠标的实际X坐标；给出的相对数据作为移动的mickeys数。一个mickey表示鼠标移动的数量，表明鼠标已经移动。</param>
        /// <param name="dy">指定鼠标沿y轴的绝对位置或者从上次鼠标事件产生以来移动的数量，依赖于MouseEventFlag.Absolute的设置。给出的绝对数据作为鼠标的实际y坐标，给出的相对数据作为移动的mickeys数。一个mickey表示鼠标移动的数量，表明鼠标已经移动。</param>
        /// <param name="data">如果dwFlags为MouseEventFlag.Wheel，则dwData指定鼠标轮移动的数量。正值表明鼠标轮向前转动，即远离用户的方向；负值表明鼠标轮向后转动，即朝向用户。一个轮击定义为WHEEL_DELTA，即120。如果dwFlags不是MouseEventFlag.Wheel，则dWData应为零。</param>
        /// <param name="extraInfo">指定与鼠标事件相关的附加32位值。应用程序调用函数GetMessageExtraInfo来获得此附加信息。</param>
        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, UIntPtr extraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetCursorPos(out Point pt);

        /// <summary>
        /// 获取当前的鼠标坐标
        /// </summary>
        /// <returns></returns>
        public static Point GetCurrentMousePoint()
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }
            
            if (GetCursorPos(out var p))
            {
                WindowsApi.WriteLog($"{nameof(GetCurrentMousePoint)} {p}");
                return p;
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(GetCurrentMousePoint)} operating failed.");
                return new Point();
            }
        }
    }
}
