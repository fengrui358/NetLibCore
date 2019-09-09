using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XFeatures2D;
using static FrHello.NetLib.Core.Windows.Windows.BitmapMatchOptions;
using Point = System.Drawing.Point;

namespace FrHello.NetLib.Core.Windows.Windows
{
    /// <summary>
    /// ScreenApi
    /// </summary>
    public class ScreenApi
    {
        private readonly Lazy<MouseApi> _innerMouseApi = new Lazy<MouseApi>(() => new MouseApi());

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDc, int x, int y, int nWidth, int nHeight, IntPtr hSrcDc, int xSrc,
            int ySrc, int dwRop);

        internal ScreenApi()
        {
        }

        /// <summary>
        /// 位图匹配算法
        /// </summary>
        public BitmapMatchOption BitmapMatchOption { get; set; } = BitmapMatchOptions.TemplateMatch;

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
                    BitBlt(hDc, 0, 0, 1, 1, hSrcDc, point.X, point.Y, (int) CopyPixelOperation.SourceCopy);
                    dest.ReleaseHdc();
                    src.ReleaseHdc();
                }
            }

            var color = screenPixel.GetPixel(0, 0);
            WindowsApi.WriteLog(
                $"{nameof(GetColorAt)} {nameof(point.X)}:{point.X},{nameof(point.Y)}:{point.Y} {nameof(color)}:{color}");

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
        public async Task<bool> WaitColorNotAt(Point point, Color wantColor, TimeSpan? timeOut = null,
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

        /// <summary>
        /// Scan color first display location on screen
        /// </summary>
        /// <param name="wantColor">Want match color</param>
        /// <param name="screen">The screen want to scanning</param>
        /// <param name="bounds">bounds</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target color location</returns>
        public async Task<Point?> ScanColorLocation(Color wantColor, Screen screen, Rectangle? bounds = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
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

            var realRectangle = GetValidIntersectRectangle(screen, bounds);
            using (var screenPixel = await InnerScreenCapture(realRectangle, linkedToken))
            {
                if (screenPixel != null)
                {
                    linkedToken.ThrowIfCancellationRequested();

                    var point = await ScanColorLocation(wantColor, screenPixel, null, linkedToken);
                    if (point != null)
                    {
                        var point2 = point.Value;
                        point2.Offset(realRectangle.Location);

                        return point2;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Wait to scan color first display location on screen
        /// </summary>
        /// <param name="wantColor">Want match color</param>
        /// <param name="screen">The screen want to scanning</param>
        /// <param name="bounds">bounds</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target color location</returns>
        public async Task<Point?> WaitScanColorLocation(Color wantColor, Screen screen, Rectangle? bounds = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
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

            return await Task.Run(async () =>
            {
                while (true)
                {
                    linkedToken.ThrowIfCancellationRequested();

                    var point = await ScanColorLocation(wantColor, screen, bounds, null, linkedToken);

                    if (point != null)
                    {
                        return point;
                    }
                }
            }, linkedToken);
        }

        /// <summary>
        /// Scan bitmap location
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="screen">screen</param>
        /// <param name="bounds">bounds</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target bitmap location</returns>
        public async Task<Rectangle?> ScanBitmapLocation(Bitmap wantBitmap, Screen screen, Rectangle? bounds = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
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

            var realRectangle = GetValidIntersectRectangle(screen, bounds);
            using (var screenPixel = await InnerScreenCapture(realRectangle, linkedToken))
            {
                if (screenPixel != null)
                {
                    linkedToken.ThrowIfCancellationRequested();

                    var rectangle = await ScanBitmapLocation(wantBitmap, screenPixel, null, linkedToken);
                    if (rectangle != null)
                    {
                        var rectangle2 = rectangle.Value;
                        rectangle2.Offset(realRectangle.Location);

                        return rectangle2;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Wait scan bitmap location
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="screen">screen</param>
        /// <param name="bounds">bounds</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        public async Task<Rectangle?> WaitScanBitmapLocation(Bitmap wantBitmap, Screen screen, Rectangle? bounds = null,
            TimeSpan? timeOut = null, CancellationToken cancellationToken = default)
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

            return await Task.Run(async () =>
            {
                while (true)
                {
                    linkedToken.ThrowIfCancellationRequested();

                    var rectangle = await ScanBitmapLocation(wantBitmap, screen, bounds, null, linkedToken);

                    if (rectangle != null)
                    {
                        return rectangle;
                    }
                }
            }, linkedToken);
        }

        /// <summary>
        /// Scan color first display location
        /// </summary>
        /// <param name="wantColor">Want match color</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target color location</returns>
        public async Task<Point?> ScanColorLocation(Color wantColor, Bitmap bitmap, TimeSpan? timeOut = null,
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

            return await Task.Run(() =>
            {
                var index = 0;

                for (var x = 0; x < bitmap.Size.Width; x++)
                {
                    for (var y = 0; y < bitmap.Size.Height; y++)
                    {
                        //降低检查频率
                        if (index++ % 10 == 0)
                        {
                            linkedToken.ThrowIfCancellationRequested();
                        }

                        if (bitmap.GetPixel(x, y) == wantColor)
                        {
                            return new Point?(new Point(x, y));
                        }
                    }
                }

                return null;
            }, cancellationToken);
        }

        /// <summary>
        /// Scan color all display locations
        /// </summary>
        /// <param name="wantColor">Want match color</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target color locations</returns>
        public async Task<Point[]> ScanColorLocations(Color wantColor, Bitmap bitmap, TimeSpan? timeOut = null,
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

            return await Task.Run(() =>
            {
                var result = new List<Point>();

                var index = 0;
                for (var x = 0; x < bitmap.Size.Width; x++)
                {
                    for (var y = 0; y < bitmap.Size.Height; y++)
                    {
                        //降低检查频率
                        if (index++ % 10 == 0)
                        {
                            linkedToken.ThrowIfCancellationRequested();
                        }

                        if (bitmap.GetPixel(x, y) == wantColor)
                        {
                            result.Add(new Point(x, y));
                        }
                    }
                }

                return result.ToArray();
            }, cancellationToken);
        }

        /// <summary>
        /// Scan bitmap location
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target bitmap location</returns>
        public async Task<Rectangle?> ScanBitmapLocation(Bitmap wantBitmap, Bitmap bitmap, TimeSpan? timeOut = null,
            CancellationToken cancellationToken = default)
        {
            if (WindowsApi.Delay.HasValue)
            {
                await Task.Delay(WindowsApi.Delay.Value, cancellationToken);
            }

            if (wantBitmap.Width == 0 || wantBitmap.Height == 0 || bitmap.Width == 0 || bitmap.Height == 0)
            {
                return null;
            }

            var linkedToken =
                timeOut == null
                    ? cancellationToken
                    : CancellationTokenSource
                        .CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(timeOut.Value).Token)
                        .Token;

            if (BitmapMatchOption == BitmapMatchOptions.TemplateMatch)
            {
                return await TemplateMatchLocation(wantBitmap, bitmap, (TemplateMatch) BitmapMatchOption, linkedToken);
            }
            else if (BitmapMatchOption == BitmapMatchOptions.SiftMatch)
            {
                return await SiftMatchLocation(wantBitmap, bitmap, linkedToken);
            }
            else if (BitmapMatchOption == BitmapMatchOptions.SurfMatch)
            {
                return await SurfMatchLocation(wantBitmap, bitmap, (SurfMatch) BitmapMatchOption, linkedToken);
            }
            else if (BitmapMatchOption == BitmapMatchOptions.Precision)
            {
                return await PrecisionMatchLocation(wantBitmap, bitmap, linkedToken);
            }

            return null;
        }

        /// <summary>
        /// Template match
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="templateMatch">template match option</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target bitmap location</returns>
        private async Task<Rectangle?> TemplateMatchLocation(Bitmap wantBitmap, Bitmap bitmap,
            TemplateMatch templateMatch, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var srcMat = bitmap.ToMat())
                    using (var dstMat = wantBitmap.ToMat())
                    using (var outArray = OutputArray.Create(srcMat))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        Cv2.MatchTemplate(srcMat, dstMat, outArray, templateMatch.TemplateMatchModel);

                        cancellationToken.ThrowIfCancellationRequested();

                        Cv2.MinMaxLoc(InputArray.Create(outArray.GetMat()), out _,
                            out var maxValue, out _, out var point);

                        if (maxValue >= templateMatch.Threshold)
                        {
                            var rectangle =
                                new Rectangle?(new Rectangle(point.X, point.Y, wantBitmap.Width, wantBitmap.Height));
                            WindowsApi.WriteLog(
                                $"{nameof(TemplateMatchLocation)} match success, {nameof(maxValue)}:{maxValue}, {rectangle}");

                            return rectangle;
                        }
                        else
                        {
                            WindowsApi.WriteLog(
                                $"{nameof(TemplateMatchLocation)} match failed, {nameof(maxValue)}:{maxValue}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    WindowsApi.WriteLog($"{nameof(TemplateMatchLocation)} ErrorMessage:{ex.Message}");
                }

                return null;
            }, cancellationToken);
        }

        /// <summary>
        /// Sift match
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target bitmap location</returns>
        private async Task<Rectangle?> SiftMatchLocation(Bitmap wantBitmap, Bitmap bitmap,
            CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var matSrc = bitmap.ToMat())
                    using (var matTo = wantBitmap.ToMat())
                    using (var matSrcRet = new Mat())
                    using (var matToRet = new Mat())
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        KeyPoint[] keyPointsSrc, keyPointsTo;
                        using (var sift = SIFT.Create())
                        {
                            sift.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
                            sift.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
                        }

                        cancellationToken.ThrowIfCancellationRequested();

                        using (var bfMatcher = new BFMatcher())
                        {
                            var matches = bfMatcher.KnnMatch(matSrcRet, matToRet, k: 2);

                            cancellationToken.ThrowIfCancellationRequested();

                            var pointsSrc = new List<Point2f>();
                            var pointsDst = new List<Point2f>();
                            foreach (var items in matches.Where(x => x.Length > 1))
                            {
                                if (items[0].Distance < 0.5 * items[1].Distance)
                                {
                                    pointsSrc.Add(keyPointsSrc[items[0].QueryIdx].Pt);
                                    pointsDst.Add(keyPointsTo[items[0].TrainIdx].Pt);
                                }
                            }

                            if (pointsSrc.Count > 0 && pointsDst.Count > 0)
                            {
                                var location = pointsSrc[0] - pointsDst[0];

                                var rectangle =
                                    new Rectangle?(new Rectangle((int) location.X, (int) location.Y, wantBitmap.Width,
                                        wantBitmap.Height));
                                WindowsApi.WriteLog(
                                    $"{nameof(SiftMatchLocation)} match success, match count:{pointsSrc.Count}, {rectangle}");

                                return rectangle;
                            }
                            else
                            {
                                WindowsApi.WriteLog(
                                    $"{nameof(SiftMatchLocation)} match failed");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WindowsApi.WriteLog($"{nameof(SiftMatchLocation)} ErrorMessage:{ex.Message}");
                }

                return null;
            }, cancellationToken);
        }

        /// <summary>
        /// Surf match
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="surfMatch">surf match option</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target bitmap location</returns>
        private async Task<Rectangle?> SurfMatchLocation(Bitmap wantBitmap, Bitmap bitmap, SurfMatch surfMatch,
            CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var matSrc = bitmap.ToMat())
                    using (var matTo = wantBitmap.ToMat())
                    using (var matSrcRet = new Mat())
                    using (var matToRet = new Mat())
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        KeyPoint[] keyPointsSrc, keyPointsTo;
                        using (var surf = SURF.Create(surfMatch.HessianThreshold, 4, 3, true, true))
                        {
                            surf.DetectAndCompute(matSrc, null, out keyPointsSrc, matSrcRet);
                            surf.DetectAndCompute(matTo, null, out keyPointsTo, matToRet);
                        }

                        cancellationToken.ThrowIfCancellationRequested();

                        using (var flnMatcher = new FlannBasedMatcher())
                        {
                            var matches = flnMatcher.Match(matSrcRet, matToRet);

                            cancellationToken.ThrowIfCancellationRequested();

                            //求最小最大距离
                            var minDistance = 1000d; //反向逼近
                            var maxDistance = 0d;
                            for (int i = 0; i < matSrcRet.Rows; i++)
                            {
                                var distance = matches[i].Distance;
                                if (distance > maxDistance)
                                {
                                    maxDistance = distance;
                                }

                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                }
                            }

                            var pointsSrc = new List<Point2f>();
                            var pointsDst = new List<Point2f>();

                            for (int i = 0; i < matSrcRet.Rows; i++)
                            {
                                double distance = matches[i].Distance;
                                if (distance < Math.Max(minDistance * 2, 0.02))
                                {
                                    pointsSrc.Add(keyPointsSrc[matches[i].QueryIdx].Pt);
                                    pointsDst.Add(keyPointsTo[matches[i].TrainIdx].Pt);
                                }
                            }

                            if (pointsSrc.Count > 0 && pointsDst.Count > 0)
                            {
                                var location = pointsSrc[0] - pointsDst[0];

                                var rectangle =
                                    new Rectangle?(new Rectangle((int) location.X, (int) location.Y, wantBitmap.Width,
                                        wantBitmap.Height));
                                WindowsApi.WriteLog(
                                    $"{nameof(SurfMatchLocation)} match success, {nameof(maxDistance)}:{maxDistance};{nameof(minDistance)}:{minDistance} match count:{pointsSrc.Count}, {rectangle}");

                                return rectangle;
                            }
                            else
                            {
                                WindowsApi.WriteLog(
                                    $"{nameof(SurfMatchLocation)} match failed, {nameof(maxDistance)}:{maxDistance};{nameof(minDistance)}:{minDistance}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WindowsApi.WriteLog($"{nameof(SurfMatchLocation)} ErrorMessage:{ex.Message}");
                }

                return null;
            }, cancellationToken);
        }

        /// <summary>
        /// Precision match
        /// </summary>
        /// <param name="wantBitmap">Want match bitmap</param>
        /// <param name="bitmap">target bitmap</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Target bitmap location</returns>
        private async Task<Rectangle?> PrecisionMatchLocation(Bitmap wantBitmap, Bitmap bitmap,
            CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var index = 0;

                for (var x = 0; x <= bitmap.Size.Width - wantBitmap.Width; x++)
                {
                    for (var y = 0; y <= bitmap.Size.Height - wantBitmap.Height; y++)
                    {
                        var isMatch = true;

                        for (var x2 = 0; x2 < wantBitmap.Size.Width; x2++)
                        {
                            for (var y2 = 0; y2 < wantBitmap.Size.Height; y2++)
                            {
                                //降低检查频率
                                if (index++ % 10 == 0)
                                {
                                    cancellationToken.ThrowIfCancellationRequested();
                                }

                                if (bitmap.GetPixel(x + x2, y + y2) != wantBitmap.GetPixel(x2, y2))
                                {
                                    isMatch = false;
                                    break;
                                }

                                if (x2 == wantBitmap.Size.Width - 1 && y2 == wantBitmap.Size.Height - 1)
                                {
                                    var rectangle =
                                        new Rectangle?(new Rectangle(x, y, wantBitmap.Width, wantBitmap.Height));

                                    WindowsApi.WriteLog($"{nameof(PrecisionMatchLocation)} match success, {rectangle}");
                                    return rectangle;
                                }
                            }

                            if (!isMatch)
                            {
                                break;
                            }
                        }
                    }
                }

                WindowsApi.WriteLog($"{nameof(PrecisionMatchLocation)} match failed");
                return null;
            }, cancellationToken);
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
        public async Task<Stream> ScreenCapture(Screen screen, ImageFormat imageFormat = null, Rectangle? bounds = null,
            TimeSpan? timeOut = null,
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

            using (var screenPixel = await InnerScreenCapture(GetValidIntersectRectangle(screen, bounds), linkedToken))
            {
                if (screenPixel != null)
                {
                    linkedToken.ThrowIfCancellationRequested();
                    var stream = new MemoryStream();

                    if (imageFormat != null)
                    {
                        screenPixel.Save(stream, imageFormat);
                        WindowsApi.WriteLog(
                            $"{nameof(ScreenCapture)} save to stream with {nameof(imageFormat)}:{imageFormat}");
                    }
                    else
                    {
                        screenPixel.Save(stream, ImageFormat.Bmp);
                        WindowsApi.WriteLog(
                            $"{nameof(ScreenCapture)} save to stream with {nameof(imageFormat)}:{ImageFormat.MemoryBmp}");
                    }

                    return stream;
                }
            }

            return null;
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
        public async Task ScreenCapture(Screen screen, string filePath, ImageFormat imageFormat = null,
            Rectangle? bounds = null, TimeSpan? timeOut = null,
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

            using (var screenPixel = await InnerScreenCapture(GetValidIntersectRectangle(screen, bounds), linkedToken))
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
        /// <param name="realRectangle">The real rectangle</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>not null</returns>
        private async Task<Bitmap> InnerScreenCapture(Rectangle realRectangle,
            CancellationToken cancellationToken = default)
        {
            if (realRectangle.Width > 0 && realRectangle.Height > 0)
            {
                return await Task.Run(() =>
                {
                    var screenPixel = new Bitmap(realRectangle.Width, realRectangle.Height,
                        PixelFormat.Format32bppArgb);
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var dest = Graphics.FromImage(screenPixel))
                    {
                        using (var src = Graphics.FromHwnd(IntPtr.Zero))
                        {
                            var hSrcDc = src.GetHdc();
                            var hDc = dest.GetHdc();
                            BitBlt(hDc, 0, 0, realRectangle.Width, realRectangle.Height, hSrcDc, realRectangle.X,
                                realRectangle.Y, (int) CopyPixelOperation.SourceCopy);
                            dest.ReleaseHdc();
                            src.ReleaseHdc();
                        }
                    }

                    WindowsApi.WriteLog($"{nameof(InnerScreenCapture)} {nameof(realRectangle)}:{realRectangle}");

                    return screenPixel;
                }, cancellationToken);
            }
            else
            {
                WindowsApi.WriteLog($"{nameof(InnerScreenCapture)} failed. {nameof(realRectangle)}:{realRectangle}");

                throw new InvalidOperationException(
                    $"Can't capture from {nameof(realRectangle)} is {realRectangle}");
            }
        }

        /// <summary>
        /// Get valid intersect rectangle
        /// </summary>
        /// <param name="targetRectangle">target rectangle</param>
        /// <param name="bounds">bounds</param>
        /// <returns>Intersect rectangle</returns>
        private Rectangle GetValidIntersectRectangle(Rectangle targetRectangle, Rectangle? bounds = null)
        {
            if (bounds != null)
            {
                var innerBounds = bounds.Value;
                innerBounds.Offset(targetRectangle.Location);
                targetRectangle.Intersect(innerBounds);
            }

            return targetRectangle;
        }

        /// <summary>
        /// Get valid intersect rectangle
        /// </summary>
        /// <param name="targetScreen">target screen</param>
        /// <param name="bounds">bounds</param>
        /// <returns>Intersect rectangle</returns>
        private Rectangle GetValidIntersectRectangle(Screen targetScreen, Rectangle? bounds = null)
        {
            return GetValidIntersectRectangle(targetScreen.Bounds, bounds);
        }
    }
}