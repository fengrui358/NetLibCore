namespace FrHello.NetLib.Core.Security
{
    /// <summary>
    /// 安全辅助的全局选项
    /// </summary>
    public class GlobalSecurityOptions
    {
        /// <summary>
        /// 分段尺寸，默认100mb
        /// </summary>
        private static int _segmentSize = 100 * 1024 * 1024;

        /// <summary>
        /// 用于计算的分段尺寸，默认100mb
        /// </summary>
        private static int _segmentSizeForComputer = 1 * 1024 * 1024;

        /// <summary>
        /// 分段尺寸，默认100mb
        /// </summary>
        public static int SegmentSize
        {
            get => _segmentSize;
            set
            {
                if (value < SegmentSizeForComputer)
                {
                    _segmentSize = SegmentSizeForComputer;
                }
                else
                {
                    _segmentSize = value;
                }
            }
        }

        /// <summary>
        /// 用于计算的分段尺寸，默认1mb
        /// </summary>
        public static int SegmentSizeForComputer
        {
            get => _segmentSizeForComputer;
            set
            {
                if (value > SegmentSize)
                {
                    _segmentSizeForComputer = SegmentSize;
                }
                else
                {
                    _segmentSizeForComputer = value;
                }
            }
        }
    }
}