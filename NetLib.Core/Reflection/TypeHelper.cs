using System;
using System.Linq;
using System.Reflection;

namespace FrHello.NetLib.Core.Reflection
{
    /// <summary>
    /// 类型辅助
    /// </summary>
    public static class TypeHelper
    {
        private const string TryParse = "TryParse";
        private const string Parse = "Parse";

        /// <summary>
        /// 判断是否为可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 获取可空类型对应的真正类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNullableInnerType(this Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments().FirstOrDefault() : type;
        }

        /// <summary>
        /// 是否为指定类型子类，包括泛型子类的情况
        /// </summary>
        /// <param name="type">子类型</param>
        /// <param name="superType">父类型</param>
        /// <param name="recursive">递归查找</param>
        /// <returns></returns>
        public static bool IsInheritedFrom(this Type type, Type superType, bool recursive = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (superType == null)
            {
                throw new ArgumentNullException(nameof(superType));
            }

            type = GetNullableInnerType(type);
            superType = GetNullableInnerType(superType);

            if (type == superType)
            {
                return false;
            }

            if (superType.IsAssignableFrom(type))
            {
                return true;
            }
            else
            {
                if (type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == superType)
                {
                    return true;
                }
                else
                {
                    if (recursive && type.BaseType != null && type.BaseType != typeof(object))
                    {
                        return type.BaseType.IsInheritedFrom(superType);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
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
        /// 类型是否有默认非静态Public无参构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否有默认构造函数</returns>
        public static bool HasDefaultConstructor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var constructors = type.GetConstructors()
                .Where(s => s.IsPublic && !s.IsAbstract && !s.IsStatic && !s.GetParameters().Any());

            return constructors.Any();
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
            if (IsNullableType(destType))
            {
                destType = Nullable.GetUnderlyingType(destType);
            }

            if (sourceType == destType)
            {
                return value;
            }

            if (destType != null && destType.IsEnum)
            {
                if (int.TryParse(value.ToString(), out var enumInt))
                {
                    return System.Enum.ToObject(destType, enumInt);
                }
                else
                {
                    return System.Enum.Parse(destType, value.ToString());
                }
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