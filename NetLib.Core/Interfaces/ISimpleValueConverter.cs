namespace FrHello.NetLib.Core.Interfaces
{
    /// <summary>
    /// 简单的类型转换
    /// </summary>
    public interface ISimpleValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="source">来源值</param>
        /// <returns>目标值</returns>
        object Convert(object source);
    }

    /// <summary>
    /// 简单的类型转换
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public abstract class SimpleValueConverter<TSource, TDestination> : ISimpleValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="source">来源值</param>
        /// <returns>目标值</returns>
        public abstract TDestination ConvertFun(TSource source);

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object Convert(object source)
        {
            return ConvertFun((TSource) source);
        }
    }
}
