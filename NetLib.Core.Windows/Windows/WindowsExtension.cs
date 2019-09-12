using System.Drawing;
using System.Windows.Forms;

namespace FrHello.NetLib.Core.Windows.Windows
{
    /// <summary>
    /// WindowsExtension
    /// </summary>
    public static class WindowsExtension
    {
        /// <summary>
        /// Get center point from a rectangle
        /// </summary>
        /// <param name="rectangle">Target rectangle</param>
        /// <returns>The center point</returns>
        public static Point GetCenterPoint(this Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        /// <summary>
        /// Get a row from screen
        /// </summary>
        /// <param name="screen">screen</param>
        /// <param name="verticalOffset">verticalOffset</param>
        /// <param name="height">height</param>
        /// <returns>Rectangle</returns>
        public static Rectangle GetScreenRow(this Screen screen, int verticalOffset, int height = 1)
        {
            if (verticalOffset < 0)
            {
                verticalOffset = 0;
            }

            if (height <= 0)
            {
                height = 1;
            }

            var maxHeight = screen.Bounds.Height - verticalOffset;

            if (maxHeight <= 0)
            {
                height = 1;
            }
            else if(maxHeight < height)
            {
                height = maxHeight;
            }

            if (verticalOffset > screen.Bounds.Height)
            {
                //超出边界
                verticalOffset = screen.Bounds.Height;
            }

            return new Rectangle(0, verticalOffset, screen.Bounds.Width, height);
        }

        /// <summary>
        /// Get a column from screen
        /// </summary>
        /// <param name="screen">screen</param>
        /// <param name="horizontalOffset">horizontalOffset</param>
        /// <param name="width">width</param>
        /// <returns>Rectangle</returns>
        public static Rectangle GetScreenColumn(this Screen screen, int horizontalOffset, int width = 1)
        {
            if (horizontalOffset < 0)
            {
                horizontalOffset = 0;
            }

            if (width <= 0)
            {
                width = 1;
            }

            var maxWidth = screen.Bounds.Width - horizontalOffset;

            if (maxWidth <= 0)
            {
                width = 1;
            }
            else if (maxWidth < width)
            {
                width = maxWidth;
            }

            if (horizontalOffset > screen.Bounds.Width)
            {
                //超出边界
                horizontalOffset = screen.Bounds.Width;
            }

            return new Rectangle(horizontalOffset, 0, width, screen.Bounds.Height);
        }
    }
}
