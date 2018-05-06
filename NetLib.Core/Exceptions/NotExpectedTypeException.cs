using System;

namespace FrHello.NetLib.Core.Exceptions
{
    /// <summary>
    /// 不是期望的类型
    /// </summary>
    public class NotExpectedTypeException : Exception
    {
        /// <summary>
        /// 期望类型
        /// </summary>
        public Type ExpectedType { get; }

        /// <summary>
        /// 实际类型
        /// </summary>
        public Type ActualType { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public override string Message
        {
            get
            {
                var message = $"Inconsistent parameter type, the actual type is {ActualType}";
                message = string.Concat(message, ExpectedType == null ? "." : $" but the expected type is {ExpectedType}.");

                return message;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="actualType">实际类型</param>
        public NotExpectedTypeException(Type actualType)
        {
            ActualType = actualType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expectedType">期望类型</param>
        /// <param name="actualType">实际类型</param>
        public NotExpectedTypeException(Type expectedType, Type actualType)
        {
            ExpectedType = expectedType;
            ActualType = actualType;
        }
    }
}
