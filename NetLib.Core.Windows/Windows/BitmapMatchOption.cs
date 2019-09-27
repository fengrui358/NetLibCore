using OpenCvSharp;

namespace FrHello.NetLib.Core.Windows.Windows
{
    /// <summary>
    /// 位图匹配算法
    /// </summary>
    public abstract class BitmapMatchOption
    {
    }

    /// <summary>
    /// 精确模式
    /// </summary>
    public sealed class Precision : BitmapMatchOption
    {
        internal Precision()
        { }
    }

    /// <summary>
    /// 模板匹配
    /// </summary>
    public sealed class TemplateMatch : BitmapMatchOption
    {
        /// <summary>
        /// 匹配阈值
        /// </summary>
        public double Threshold { get; set; } = 0.9;

        /// <summary>
        /// 匹配模式
        /// </summary>
        public TemplateMatchModes TemplateMatchModel { get; set; } = TemplateMatchModes.CCoeffNormed;

        internal TemplateMatch()
        { }
    }

    /// <summary>
    /// Sift算法
    /// </summary>
    public sealed class SiftMatch : BitmapMatchOption
    {
        internal SiftMatch()
        { }
    }

    /// <summary>
    /// Surf算法
    /// </summary>
    public sealed class SurfMatch : BitmapMatchOption
    {
        /// <summary>
        /// Only features with key point.hessian larger than that are extracted.
        /// </summary>
        public double HessianThreshold { get; set; } = 400;

        internal SurfMatch()
        { }
    }

    /// <summary>
    /// 位图匹配算法
    /// 具体可参考：https://www.cnblogs.com/StupidsCat/p/11453088.html
    /// </summary>
    public class BitmapMatchOptions
    {
        /// <summary>
        /// 模板匹配，常用，首选CCorrNormed
        /// </summary>
        public static BitmapMatchOption TemplateMatch { get; } = new TemplateMatch();

        /// <summary>
        /// 适用于不同分辨率，测试优于Surf算法
        /// </summary>
        public static BitmapMatchOption SiftMatch { get; } = new SiftMatch();

        /// <summary>
        /// 适用于不同分辨率
        /// </summary>
        public static BitmapMatchOption SurfMatch { get; } = new SurfMatch();

        /// <summary>
        /// 精确模式，像素逐一比较，效率最差
        /// </summary>
        public static BitmapMatchOption Precision { get; } = new Precision();
    }
}
