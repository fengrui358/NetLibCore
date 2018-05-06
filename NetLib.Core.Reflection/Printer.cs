using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FrHello.NetLib.Core.Reflection
{
    /// <summary>
    /// 打印输出类型的静态字段、属性、方法信息
    /// </summary>
    public static class Printer
    {
        /// <summary>
        /// 打印类型的相关信息
        /// </summary>
        /// <param name="typeInfo">类型</param>
        /// <param name="includeNonPublic">是否包括非公有信息</param>
        /// <returns>打印信息</returns>
        public static string Output(TypeInfo typeInfo, bool includeNonPublic = false)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            return Output((Type) typeInfo);
        }

        /// <summary>
        /// 打印输出类型的静态字段、属性、方法信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="includeNonPublic">是否包括非公有信息</param>
        /// <returns>打印信息</returns>
        public static string Output(Type type, bool includeNonPublic = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var outPutValues = new List<Tuple<string, string>>();
            foreach (var reflectionInfo in GetReflectionInfos(
                MemberTypes.Field | MemberTypes.Property | MemberTypes.Method, type,
                includeNonPublic: includeNonPublic))
            {
                outPutValues.Add(reflectionInfo);
            }



            return "";
        }

        /// <summary>
        /// 打印实例的相关信息
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="includeNonPublic">是否包括非公有信息</param>
        /// <returns>打印信息</returns>
        public static string Output(object obj, bool includeNonPublic = false)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var type = obj.GetType();

            return "";
        }

        /// <summary>
        /// 获取类型的静态相关字段、属性、方法
        /// </summary>
        /// <param name="memberType">要获取的类型</param>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <param name="includeNonPublic">是否包括非公有信息</param>
        /// <returns>打印信息</returns>
        private static IEnumerable<Tuple<string, string>> GetReflectionInfos(MemberTypes memberType, Type type,
            object obj = null, bool includeNonPublic = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (obj != null && obj.GetType() != type)
            {
                throw new Exception($"{nameof(obj)} inconsistent parameter type with {nameof(type)}");
            }

            switch (memberType)
            {
                case MemberTypes.Field:
                    //获取静态字段
                    var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)
                        .ToList();
                    if (includeNonPublic)
                    {
                        fieldInfos.AddRange(
                            type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static));
                    }

                    //获取实例字段
                    if (obj != null)
                    {
                        fieldInfos.AddRange(
                            type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance));

                        if (includeNonPublic)
                        {
                            fieldInfos.AddRange(type.GetFields(
                                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance));
                        }
                    }

                    foreach (var fieldInfo in fieldInfos)
                    {
                        var fieldValue = fieldInfo.GetValue(obj);
                        yield return new Tuple<string, string>(fieldInfo.ToString(), fieldValue?.ToString());
                    }

                    break;
                case MemberTypes.Property:
                    //获取静态属性
                    var propertyInfos =
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).ToList();
                    if (includeNonPublic)
                    {
                        propertyInfos.AddRange(
                            type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static));
                    }

                    //获取实例属性
                    if (obj != null)
                    {
                        propertyInfos.AddRange(
                            type.GetProperties(
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance));

                        if (includeNonPublic)
                        {
                            propertyInfos.AddRange(type.GetProperties(
                                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance));
                        }
                    }

                    foreach (var propertyInfo in propertyInfos)
                    {
                        var propertyValue = propertyInfo.GetValue(obj);
                        yield return new Tuple<string, string>(propertyInfo.ToString(), propertyValue?.ToString());
                    }

                    break;
                case MemberTypes.Method:
                    //获取静态方法
                    var methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)
                        .ToList();
                    if (includeNonPublic)
                    {
                        methodInfos.AddRange(
                            type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static));
                    }

                    //获取实例方法
                    if (obj != null)
                    {
                        methodInfos.AddRange(
                            type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance));

                        if (includeNonPublic)
                        {
                            methodInfos.AddRange(type.GetMethods(
                                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance));
                        }
                    }

                    foreach (var methodInfo in methodInfos)
                    {
                        //如果没有参数并且有返回值，再求值
                        if (!methodInfo.GetGenericArguments().Any() && !methodInfo.ContainsGenericParameters &&
                            methodInfo.ReturnParameter != null)
                        {
                            var returnValue = methodInfo.Invoke(obj, null);
                            yield return new Tuple<string, string>(methodInfo.ToString(), returnValue?.ToString());
                        }
                    }

                    break;
                default:
                    throw new NotSupportedException(nameof(memberType));
            }
        }

        /// <summary>
        /// 获取打印信息
        /// </summary>
        /// <param name="values">需要打印的内容</param>
        /// <returns>返回打印字符的拼接</returns>
        private static StringBuilder GetPrintStringBuilder(IEnumerable<Tuple<string, string>> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var sb = new StringBuilder();

            return sb;
        }
    }
}