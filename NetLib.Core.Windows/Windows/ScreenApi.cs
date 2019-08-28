using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrHello.NetLib.Core.Windows.Windows
{
    /// <summary>
    /// ScreenApi
    /// </summary>
    public class ScreenApi
    {
        private readonly Lazy<MouseApi> _innerMouseApi = new Lazy<MouseApi>(() => new MouseApi());

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        internal ScreenApi()
        {
        }

        /// <summary>
        /// PrimaryScreen
        /// </summary>
        public Screen PrimaryScreen => Screen.PrimaryScreen;

        /// <summary>
        /// AllScreens
        /// </summary>
        public Screen[] AllScreens
        {
            get { return Screen.AllScreens.OrderBy(s => s.Bounds.X).ThenBy(s => s.Bounds.Y).ToArray(); }
        }

        /// <summary>
        /// Get mouse belong screen.
        /// </summary>
        /// <returns>Current screen contain mouse</returns>
        public Screen GetMouseScreen()
        {
            return GetMouseScreen(_innerMouseApi.Value.GetCurrentMousePoint());
        }

        /// <summary>
        /// Get mouse belong screen.
        /// </summary>
        /// <param name="point">Mouse point</param>
        /// <returns>Current screen contain mouse</returns>
        public Screen GetMouseScreen(MouseApi.MousePoint point)
        {
            return Screen.AllScreens.FirstOrDefault(s => s.Bounds.Contains(new Point(point.X, point.Y)));
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="point">point</param>
        /// <returns>color</returns>
        public Color GetColorAt(Point point)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            var screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

            using (var dest = Graphics.FromImage(screenPixel))
            {
                using (var src = Graphics.FromHwnd(IntPtr.Zero))
                {
                    var hSrcDc = src.GetHdc();
                    var hDc = dest.GetHdc();
                    BitBlt(hDc, 0, 0, 1, 1, hSrcDc, point.X, point.Y, (int)CopyPixelOperation.SourceCopy);
                    dest.ReleaseHdc();
                    src.ReleaseHdc();
                }
            }

            var color = screenPixel.GetPixel(0, 0);
            WindowsApi.WriteLog($"{nameof(GetColorAt)} {nameof(point.X)}:{point.X},{nameof(point.Y)}:{point.Y} {nameof(color)}:{color}");

            return color;
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="mousePoint">point</param>
        /// <returns>color</returns>
        public Color GetColorAt(MouseApi.MousePoint mousePoint)
        {
            return GetColorAt(new Point(mousePoint.X, mousePoint.Y));
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>color</returns>
        public Color GetColorAt(int x, int y)
        {
            return GetColorAt(new Point(x, y));
        }

        /// <summary>
        /// Get color at current mouse.
        /// </summary>
        /// <returns>color</returns>
        public Color GetColorAt()
        {
            return GetColorAt(_innerMouseApi.Value.GetCurrentMousePoint());
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="point">point</param>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorAt(Point point, Color wantColor, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            Color? color;

            var linkedToken =
                timeOut == null
                    ? cancellationToken
                    : CancellationTokenSource
                        .CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeOut.Value).Token)
                        .Token;

            var getColor = false;

            await Task.Run(async () =>
            {
                using (var screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
                {
                    do
                    {
                        linkedToken.ThrowIfCancellationRequested();

                        using (var dest = Graphics.FromImage(screenPixel))
                        {
                            using (var src = Graphics.FromHwnd(IntPtr.Zero))
                            {
                                var hSrcDc = src.GetHdc();
                                var hDc = dest.GetHdc();
                                BitBlt(hDc, 0, 0, 1, 1, hSrcDc, point.X, point.Y, (int) CopyPixelOperation.SourceCopy);
                                dest.ReleaseHdc();
                                src.ReleaseHdc();
                            }
                        }

                        color = screenPixel.GetPixel(0, 0);

                        await Task.Delay(5, linkedToken);
                        WindowsApi.WriteLog(
                            $"{nameof(GetColorAt)} {nameof(point.X)}:{point.X},{nameof(point.Y)}:{point.Y} {nameof(color)}:{color.Value}");

                        if (color == wantColor)
                        {
                            getColor = true;
                            WindowsApi.WriteLog(
                                $"{nameof(WaitColorAt)} {nameof(point.X)}:{point.X},{nameof(point.Y)}:{point.Y} {nameof(color)} is {color.Value}");
                        }

                    } while (!getColor);
                }
            }, linkedToken);

            return getColor;
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="mousePoint">point</param>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorAt(MouseApi.MousePoint mousePoint, Color wantColor, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            return await WaitColorAt(new Point(mousePoint.X, mousePoint.Y), wantColor, timeOut, cancellationToken);
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorAt(int x, int y, Color wantColor, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            return await WaitColorAt(new Point(x, y), wantColor, timeOut, cancellationToken);
        }

        /// <summary>
        /// Get color at current mouse.
        /// </summary>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorAt(Color wantColor, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            return await WaitColorAt(_innerMouseApi.Value.GetCurrentMousePoint(), wantColor, timeOut,
                cancellationToken);
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="point">point</param>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorNotAt(Point point, Color wantColor, TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
        {
            if (WindowsApi.Delay.HasValue)
            {
                Thread.Sleep(WindowsApi.Delay.Value);
            }

            Color? color;

            var linkedToken =
                timeOut == null
                    ? cancellationToken
                    : CancellationTokenSource
                        .CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeOut.Value).Token)
                        .Token;

            var getColor = false;

            await Task.Run(async () =>
            {
                using (var screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
                {
                    do
                    {
                        linkedToken.ThrowIfCancellationRequested();

                        using (var dest = Graphics.FromImage(screenPixel))
                        {
                            using (var src = Graphics.FromHwnd(IntPtr.Zero))
                            {
                                var hSrcDc = src.GetHdc();
                                var hDc = dest.GetHdc();
                                BitBlt(hDc, 0, 0, 1, 1, hSrcDc, point.X, point.Y, (int) CopyPixelOperation.SourceCopy);
                                dest.ReleaseHdc();
                                src.ReleaseHdc();
                            }
                        }

                        color = screenPixel.GetPixel(0, 0);

                        await Task.Delay(5, linkedToken);
                        WindowsApi.WriteLog(
                            $"{nameof(GetColorAt)} {nameof(point.X)}:{point.X},{nameof(point.Y)}:{point.Y} {nameof(color)}:{color.Value}");

                        if (color != wantColor)
                        {
                            getColor = true;
                            WindowsApi.WriteLog(
                                $"{nameof(WaitColorAt)} {nameof(point.X)}:{point.X},{nameof(point.Y)}:{point.Y} {nameof(color)} isn't {color.Value}");
                        }

                    } while (!getColor);
                }
            }, linkedToken);

            return getColor;
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="mousePoint">point</param>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorNotAt(MouseApi.MousePoint mousePoint, Color wantColor,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
        {
            return await WaitColorNotAt(new Point(mousePoint.X, mousePoint.Y), wantColor, timeOut, cancellationToken);
        }

        /// <summary>
        /// Get color at point.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorNotAt(int x, int y, Color wantColor, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            return await WaitColorNotAt(new Point(x, y), wantColor, timeOut, cancellationToken);
        }

        /// <summary>
        /// Get color at current mouse.
        /// </summary>
        /// <param name="wantColor">want color</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>If wait unit time out, the return color is null.</returns>
        public async Task<bool> WaitColorNotAt(Color wantColor, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            return await WaitColorNotAt(_innerMouseApi.Value.GetCurrentMousePoint(), wantColor, timeOut,
                cancellationToken);
        }
    }
}
