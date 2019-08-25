using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FrHello.NetLib.Core.Windows.Windows
{
    public class MouseApi
    {
        private readonly Lazy<ScreenApi> _innerScrrenApi = new Lazy<ScreenApi>(() => new ScreenApi());
        private const int MagicNumber = 65536;

        /// <summary>
        /// 鼠标按压持续时间
        /// </summary>
        private const uint MousePressedTime = 800;

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
            public int X { get; set; }
            public int Y { get; set; }
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
        // ReSharper disable once UnusedMember.Local
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
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            var bounds = _innerScrrenApi.Value.GetMouseScreen(point).Bounds;
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, point.X * (MagicNumber / bounds.Width),
                point.Y * (MagicNumber / bounds.Height),
                0, UIntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(MouseMove)} {nameof(MousePoint.X)}:{point.X},{nameof(MousePoint.Y)}:{point.Y}");
        }

        /// <summary>
        /// 鼠标移动到相对当前鼠标的位置
        /// </summary>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        public void MouseMove(int offsetX, int offsetY)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            mouse_event(MouseEventFlag.Move, offsetX, offsetY, 0, UIntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(MouseMove)} {nameof(offsetX)}:{offsetX},{nameof(offsetY)}:{offsetY}");
        }

        /// <summary>
        /// 鼠标移动到绝对位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pressedMillionSeconds"></param>
        /// <returns></returns>
        public async Task MouseMove(MousePoint point, uint pressedMillionSeconds)
        {
            if (WindowsApi.Delay.HasValue)
            {
                await Task.Delay(WindowsApi.Delay.Value);
            }

            if (pressedMillionSeconds == 0u)
            {
                pressedMillionSeconds = MousePressedTime;
            }

            await Task.Run(async () =>
            {
                var currentPoint = GetCurrentMousePoint();

                var xDiff = point.X - currentPoint.X;
                var yDiff = point.Y - currentPoint.Y;

                //10ms移动一次
                var timeInterval = pressedMillionSeconds / 10;
                if (timeInterval == 0)
                {
                    timeInterval = 1;
                }

                var xPixel = (int)(xDiff / timeInterval);
                var yPixel = (int)(yDiff / timeInterval);

                while (currentPoint.X != point.X || currentPoint.Y != point.Y)
                {
                    if (currentPoint.X != point.X)
                    {
                        currentPoint.X += xPixel;
                        mouse_event(MouseEventFlag.Move, xPixel, 0, 0, UIntPtr.Zero);
                        await Task.Delay(10);
                    }

                    if (currentPoint.Y != point.Y)
                    {
                        currentPoint.Y += yPixel;
                        mouse_event(MouseEventFlag.Move, 0, yPixel, 0, UIntPtr.Zero);
                        await Task.Delay(10);
                    }
                }

                WindowsApi.WriteLog(
                    $"{nameof(MouseMove)} from {nameof(MousePoint.X)}:{currentPoint.X},{nameof(MousePoint.Y)}:{currentPoint.Y} to {nameof(MousePoint.X)}:{point.X},{nameof(MousePoint.Y)}:{point.Y}");
            });
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="rightButton">右键</param>
        public void MouseClick(bool rightButton = false)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            mouse_event(rightButton ? MouseEventFlag.RightDown : MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(rightButton ? MouseEventFlag.RightUp : MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);

            WindowsApi.WriteLog($"{nameof(MouseClick)} {GetButtonString(rightButton)}");
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="point">需要移动到的坐标</param>
        /// <param name="rightButton">右键</param>
        public void MouseClick(MousePoint point, bool rightButton = false)
        {
            MouseMove(point);
            MouseClick(rightButton);
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        /// <param name="rightButton">右键</param>
        public void MouseClick(int offsetX, int offsetY, bool rightButton = false)
        {
            MouseMove(offsetX, offsetY);
            MouseClick(rightButton);
        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="rightButton">右键</param>
        public void MouseDoubleClick(bool rightButton = false)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            mouse_event(rightButton ? MouseEventFlag.RightDown : MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(rightButton ? MouseEventFlag.RightUp : MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
            mouse_event(rightButton ? MouseEventFlag.RightDown : MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(rightButton ? MouseEventFlag.RightUp : MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);

            WindowsApi.WriteLog($"{nameof(MouseDoubleClick)} {GetButtonString(rightButton)}");
        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="point">需要移动到的坐标</param>
        /// <param name="rightButton">右键</param>
        public void MouseDoubleClick(MousePoint point, bool rightButton = false)
        {
            MouseMove(point);
            MouseDoubleClick(rightButton);
        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        /// <param name="rightButton">右键</param>
        public void MouseDoubleClick(int offsetX, int offsetY, bool rightButton = false)
        {
            MouseMove(offsetX, offsetY);
            MouseDoubleClick(rightButton);
        }

        /// <summary>
        /// 鼠标按压
        /// </summary>
        /// <param name="rightButton">右键</param>
        /// <param name="pressedMillionSeconds">按压时长</param>
        public async Task MousePressed(bool rightButton = false, uint pressedMillionSeconds = MousePressedTime)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            await Task.Run(async () =>
            {
                mouse_event(rightButton ? MouseEventFlag.RightDown : MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
                await Task.Delay(TimeSpan.FromMilliseconds(pressedMillionSeconds));
                mouse_event(rightButton ? MouseEventFlag.RightUp : MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);

                WindowsApi.WriteLog(
                    $"{nameof(MousePressed)} {GetButtonString(rightButton)} {nameof(MousePressedTime)}:{pressedMillionSeconds}");
            });
        }

        /// <summary>
        /// 鼠标按压
        /// </summary>
        /// <param name="point">需要移动到的坐标</param>
        /// <param name="rightButton">右键</param>
        /// <param name="pressedMillionSeconds">按压时长</param>
        public async Task MousePressed(MousePoint point, bool rightButton = false, uint pressedMillionSeconds = MousePressedTime)
        {
            MouseMove(point);
            await MousePressed(rightButton, pressedMillionSeconds);
        }

        /// <summary>
        /// 鼠标按压
        /// </summary>
        /// <param name="offsetX">offsetX</param>
        /// <param name="offsetY">offsetY</param>
        /// <param name="rightButton">右键</param>
        /// <param name="pressedMillionSeconds">按压时长</param>
        public async Task MousePressed(int offsetX, int offsetY, bool rightButton = false, uint pressedMillionSeconds = MousePressedTime)
        {
            MouseMove(offsetX, offsetY);
            await MousePressed(rightButton, pressedMillionSeconds);
        }

        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        /// <param name="wheelDelta">正值表明鼠标轮向前转动，即远离用户的方向；负值表明鼠标轮向后转动，即朝向用户。</param>
        public void MouseWheel(int wheelDelta = 500)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            mouse_event(MouseEventFlag.Wheel, 0, 0, wheelDelta, UIntPtr.Zero);
            WindowsApi.WriteLog($"{nameof(MouseWheel)} {nameof(wheelDelta)}:{wheelDelta}");
        }

        //public void MouseDrag()

        /// <summary>
        /// 获取鼠标按键字符串
        /// </summary>
        /// <param name="rightButton">右键</param>
        /// <returns>鼠标按键字符串</returns>
        private string GetButtonString(bool rightButton)
        {
            return rightButton ? "RightButton" : "LeftButton";
        }
    }
}
