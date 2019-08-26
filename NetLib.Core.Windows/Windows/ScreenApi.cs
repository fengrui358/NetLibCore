using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FrHello.NetLib.Core.Windows.Windows
{
    /// <summary>
    /// ScreenApi
    /// </summary>
    public class ScreenApi
    {
        private readonly Lazy<MouseApi> _innerMouseApi = new Lazy<MouseApi>(() => new MouseApi());

        private Lazy<Screen[]> _allScreen;

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
    }
}
