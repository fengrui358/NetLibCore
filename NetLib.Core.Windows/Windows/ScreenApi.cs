using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public Task<Point> ScanColorLocation(Color wantColor, Screen screen, Rectangle? rectangle = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
        {
            //确定扫描范围
        }

        public Task<Point> WaitScanColorLocation(Color wantColor, Screen screen, Rectangle? rectangle = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
        {
            //确定扫描范围
        }

        public Task<Rectangle?> ScanBitmapLocation(Bitmap wantBitmap, Screen screen, Rectangle? rectangle = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
        {
            //确定扫描范围
        }

        public Task<Rectangle?> WaitScanBitmapLocation(Bitmap wantBitmap, Screen screen, Rectangle? rectangle = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
        {
            //确定扫描范围
        }

        /// <summary>
        /// Screen capture
        /// </summary>
        /// <param name="screen">The screen want to capture</param>
        /// <param name="imageFormat">Save image file path</param>
        /// <param name="bounds">bounds</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        public async Task<Stream> ScreenCapture(Screen screen, ImageFormat imageFormat = null, Rectangle? bounds = null, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            if (WindowsApi.Delay.HasValue)
            {
                await Task.Delay(WindowsApi.Delay.Value, cancellationToken);
            }

            var linkedToken =
                timeOut == null
                    ? cancellationToken
                    : CancellationTokenSource
                        .CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeOut.Value).Token)
                        .Token;

            using (var screenPixel = await InnerScreenCapture(screen, bounds, linkedToken))
            {
                if (screenPixel != null)
                {
                    linkedToken.ThrowIfCancellationRequested();
                    var stream = new MemoryStream();

                    if (imageFormat != null)
                    {
                        screenPixel.Save(stream, imageFormat);
                        WindowsApi.WriteLog($"{nameof(ScreenCapture)} save to stream");
                    }
                    else
                    {
                        screenPixel.Save(stream, ImageFormat.MemoryBmp);
                        WindowsApi.WriteLog($"{nameof(ScreenCapture)} save to stream with {nameof(imageFormat)}:{imageFormat}");
                    }

                    return stream;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Screen capture
        /// </summary>
        /// <param name="screen">The screen want to capture</param>
        /// <param name="filePath">Save image file path</param>
        /// <param name="imageFormat">Image format</param>
        /// <param name="bounds">bounds</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        public async Task ScreenCapture(Screen screen, string filePath, ImageFormat imageFormat = null, Rectangle? bounds = null, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            if (WindowsApi.Delay.HasValue)
            {
                await Task.Delay(WindowsApi.Delay.Value, cancellationToken);
            }

            var linkedToken =
                timeOut == null
                    ? cancellationToken
                    : CancellationTokenSource
                        .CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeOut.Value).Token)
                        .Token;

            using (var screenPixel = await InnerScreenCapture(screen, bounds, linkedToken))
            {
                if (screenPixel != null)
                {
                    linkedToken.ThrowIfCancellationRequested();
                    if (imageFormat != null)
                    {
                        screenPixel.Save(filePath, imageFormat);
                        WindowsApi.WriteLog($"Save to {filePath} with {nameof(imageFormat)}:{imageFormat}");
                    }
                    else
                    {
                        screenPixel.Save(filePath);
                        WindowsApi.WriteLog($"Save to {filePath}");
                    }
                }
            }
        }

        /// <summary>
        /// Inner screen capture
        /// </summary>
        /// <param name="screen">The screen want to capture</param>
        /// <param name="bounds">bounds</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        private async Task<Bitmap> InnerScreenCapture(Screen screen, Rectangle? bounds = null,
            CancellationToken cancellationToken = default)
        {
            var screenBounds = screen.Bounds;

            if (bounds != null)
            {
                bounds.Value.Offset(screenBounds.Location);
                screenBounds.Intersect(bounds.Value);
            }

            if (screenBounds.Width > 0 && screenBounds.Height > 0)
            {
                return await Task.Run(() =>
                {
                    var screenPixel = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var dest = Graphics.FromImage(screenPixel))
                    {
                        using (var src = Graphics.FromHwnd(IntPtr.Zero))
                        {
                            var hSrcDc = src.GetHdc();
                            var hDc = dest.GetHdc();
                            BitBlt(hDc, 0, 0, screenBounds.Width, screenBounds.Height, hSrcDc, screenBounds.X,
                                screenBounds.Y, (int)CopyPixelOperation.SourceCopy);
                            dest.ReleaseHdc();
                            src.ReleaseHdc();
                        }
                    }

                    WindowsApi.WriteLog($"{nameof(InnerScreenCapture)} {nameof(screenBounds)}:{screenBounds}");

                    return screenPixel;
                }, cancellationToken);
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(InnerScreenCapture)} failed. {nameof(screenBounds)}:{screenBounds}");
                return null;
            }
        }

        /// <summary>
        /// Get valid intersect rectangle
        /// </summary>
        /// <param name="targetRectangle">target rectangle</param>
        /// <param name="bounds">bounds</param>
        /// <returns>Intersect rectangle</returns>
        [Obsolete]
        private Rectangle GetValidIntersectRectangle(Rectangle targetRectangle, Rectangle bounds)
        {
            bounds.Offset(targetRectangle.Location);

            var x = targetRectangle.X;
            var y = targetRectangle.Y;

            if (bounds.X > x && bounds.X < (x + targetRectangle.Width))
            {
                x = bounds.X;
            }

            if (bounds.Y > y && bounds.Y < (y + targetRectangle.Height))
            {
                y = bounds.Y;
            }

            int width;
            int height;
            var maxTargetRectangleX = targetRectangle.X + targetRectangle.Width;
            var maxTargetRectangleY = targetRectangle.Y + targetRectangle.Height;

            var maxBoundsRectangleX = bounds.X + bounds.Width;
            var maxBoundsRectangleY = bounds.Y + bounds.Height;

            if (maxBoundsRectangleX > x && maxBoundsRectangleX < maxTargetRectangleX)
            {
                width = maxBoundsRectangleX - x;
            }
            else
            {
                width = maxTargetRectangleX - x;
            }

            if (maxBoundsRectangleY > y && maxBoundsRectangleY < maxTargetRectangleY)
            {
                height = maxBoundsRectangleY - y;
            }
            else
            {
                height = maxTargetRectangleY - y;
            }

            return new Rectangle(x, y, width, height);
        }
    }
}
