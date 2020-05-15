using System.Diagnostics;

namespace FrHello.NetLib.Core.Reflection
{
    /// <summary>
    /// 反射辅助类
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取方法调用者信息
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        public static void GetInvokerInfo(out string className, out string methodName)
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(2);//1代表上级，2代表上上级，以此类推
            var method = frame.GetMethod();

            methodName = method.Name;
            className = method.ReflectedType?.Name;
        }
    }
}
