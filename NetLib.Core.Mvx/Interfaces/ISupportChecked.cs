namespace FrHello.NetLib.Core.Mvx.Interfaces
{
    /// <summary>
    /// 是否支持复选选中
    /// </summary>
    public interface ISupportChecked
    {
        /// <summary>
        /// 是否可以选中
        /// </summary>
        bool IsChecked { get; set; }
    }
}
