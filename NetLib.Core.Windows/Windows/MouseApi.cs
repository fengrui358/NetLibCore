using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public class MouseApi
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

        /// <summary>
        /// 内部坐标点
        /// </summary>
        public struct MousePoint
        {
            public int X { get; }
            public int Y { get; }
            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
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
        private static extern bool GetCursorPos(out MousePoint pt);

        /// <summary>
        /// 获取当前的鼠标坐标
        /// </summary>
        /// <returns></returns>
        public MousePoint GetCurrentMousePoint()
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }
            
            if (GetCursorPos(out var p))
            {
                WindowsApi.WriteLog($"{nameof(GetCurrentMousePoint)} {nameof(MousePoint.X)}:{p.X},{nameof(MousePoint.Y)}:{p.Y}");
                return p;
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(GetCurrentMousePoint)} operating failed.");
                return new MousePoint();
            }
        }

        /// <summary>
        /// 鼠标移动到绝对位置
        /// </summary>
        /// <param name="point">需要移动到的坐标</param>
        public void MouseMove(MousePoint point)
        {

        }

        /// <summary>
        /// 鼠标移动到相对当前鼠标的位置
        /// </summary>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        public void MouseMove(int offsetX, int offsetY)
        {

        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="rightButton">右键</param>
        public void MouseClick(bool rightButton)
        {

        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="rightButton">右键</param>
        public void MouseDoubleClick(bool rightButton)
        {

        }

        /// <summary>
        /// 鼠标按压
        /// </summary>
        /// <param name="rightButton">右键</param>
        /// <param name="millionSeconds">按压时长</param>
        public void MousePressed(bool rightButton, uint millionSeconds)
        {

        }

        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        /// <param name="wheelDelta">正值表明鼠标轮向前转动，即远离用户的方向；负值表明鼠标轮向后转动，即朝向用户。</param>
        public void MouseWheel(int wheelDelta = 500)
        {

        }
    }
}
