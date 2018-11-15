using System;
using System.Linq;
using System.Reflection;

namespace FrHello.NetLib.Core.Reflection
{
    /// <summary>
    /// 类型辅助
    /// </summary>
    public class TypeHelper
    {
        private const string TryParse = "TryParse";
        private const string Parse = "Parse";

        /// <summary>
        /// 判断是否为可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 转换值类型
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="type">欲转换的类型</param>
        /// <returns>转换后的值</returns>
        public static object ChangeType(object value, Type type)
        {
            return ChangeTypeImpl(value, type, false);
        }

        /// <summary>
        /// 尝试转换值类型
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="type">欲转换的类型</param>
        /// <returns>转换后的值</returns>
        public static object TryChangeType(object value, Type type)
        {
            return ChangeTypeImpl(value, type, true);
        }

        /// <summary>
        /// 根据类型改变值
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="type">欲转换的类型</param>
        /// <param name="isTry">是否尝试转换，会丢失精度</param>
        /// <returns>转换后的值</returns>
        private static object ChangeTypeImpl(object value, Type type, bool isTry)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (value == null)
            {
                return null;
            }

            var sourceType = value.GetType();

            if (sourceType == type)
            {
                return value;
            }

            var destType = type;
            Type destNullableType = null;
            if (IsNullableType(destType))
            {
                destNullableType = destType;
                destType = Nullable.GetUnderlyingType(destType);
            }

            object converterValue;

            MethodInfo parseMethodInfo;

            if (isTry)
            {
                parseMethodInfo = GetTryParseMethod(destType);
            }
            else
            {
                parseMethodInfo = GetParseMethod(destType);
            }
            
            if (parseMethodInfo != null)
            {
                //获取真值
                if (GetParseValue(parseMethodInfo, value, out var outValue))
                {
                    converterValue = outValue;
                }
                else
                {
                    converterValue =
                        Convert.ChangeType(value, destType ?? throw new ArgumentNullException(nameof(destType)));
                }
            }
            else
            {
                converterValue =
                    Convert.ChangeType(value, destType ?? throw new ArgumentNullException(nameof(destType)));
            }

            if (destNullableType != null)
            {
                //包装成可空类型
                var result = Activator.CreateInstance(destNullableType);
                result = converterValue;
                return result;
            }

            return converterValue;
        }

        /// <summary>
        /// 获取尝试转换的方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>方法</returns>
        internal static MethodInfo GetTryParseMethod(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var innerType = type;
            if (IsNullableType(type))
            {
                innerType = Nullable.GetUnderlyingType(innerType);
            }

            var methods = innerType?.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(s =>
            {
                if (s.ReturnParameter != null &&
                    (s.Name == TryParse && s.ReturnParameter.ParameterType == typeof(bool)))
                {
                    //判断传入参数
                    var parameters = s.GetParameters();
                    if (parameters.Length == 2 && parameters[0].ParameterType == typeof(string) &&
                        parameters[1].IsOut && parameters[1].ParameterType.IsByRef &&
                        parameters[1].ParameterType.Name == $"{innerType.Name}&")
                    {
                        return true;
                    }
                }

                return false;
            });

            if (methods != null)
            {
                var f = methods.FirstOrDefault();

                if (f != null)
                {
                    return f;
                }
                else
                {
                    //尝试查找Parse方法
                    return GetParseMethod(type);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取转换的方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>方法</returns>
        internal static MethodInfo GetParseMethod(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            
            var innerType = type;
            if (IsNullableType(type))
            {
                innerType = Nullable.GetUnderlyingType(innerType);
            }

            var methods = innerType?.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(s =>
            {
                if (s.ReturnParameter != null && s.Name == Parse && s.ReturnParameter.ParameterType == type)
                {
                    //判断传入参数
                    var parameters = s.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                    {
                        return true;
                    }
                }

                return false;
            });

            return methods?.FirstOrDefault();
        }

        /// <summary>
        /// 获取转换方法的转换值
        /// </summary>
        /// <param name="methodInfo">方法</param>
        /// <param name="obj">对象</param>
        /// <param name="outObj">转换后的值</param>
        /// <returns>转换是否成功</returns>
        private static bool GetParseValue(MethodInfo methodInfo, object obj, out object outObj)
        {
            outObj = null;

            if (methodInfo == null || obj == null)
            {
                return false;
            }

            if (methodInfo.Name == Parse)
            {
                outObj = methodInfo.Invoke(null, new object[] {obj.ToString()});
                return true;
            }

            if (methodInfo.Name == TryParse)
            {
                var args = new object[] { obj.ToString(), null };
                var r = (bool) methodInfo.Invoke(null, args);

                if (r)
                {
                    outObj = args[1];
                    return true;
                }
            }

            return false;
        }
    }
}