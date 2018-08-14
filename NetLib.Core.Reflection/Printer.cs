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

            return Output((Type) typeInfo, includeNonPublic);
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

            return GetPrintStringBuilder(outPutValues).ToString();
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
            var outPutValues = new List<Tuple<string, string>>();
            foreach (var reflectionInfo in GetReflectionInfos(
                MemberTypes.Field | MemberTypes.Property | MemberTypes.Method, type, obj, includeNonPublic))
            {
                outPutValues.Add(reflectionInfo);
            }

            return GetPrintStringBuilder(outPutValues).ToString();
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

            var result = new List<Tuple<string, string>>();

            if ((memberType & MemberTypes.Field) != 0)
            {
                #region Field

                //获取静态字段
                var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .ToList();
                if (includeNonPublic)
                {
                    fieldInfos.AddRange(
                        type.GetFields(BindingFlags.NonPublic | BindingFlags.Static));
                }

                //获取实例字段
                if (obj != null)
                {
                    fieldInfos.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public));

                    if (includeNonPublic)
                    {
                        fieldInfos.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic));
                    }
                }

                foreach (var fieldInfo in fieldInfos)
                {
                    string value;
                    try
                    {
                        var fieldValue = fieldInfo.GetValue(obj);
                        value = fieldValue == null ? string.Empty : fieldValue.ToString().Trim().Ellipsis();
                    }
                    catch(Exception)
                    {
                        value = "error";
                    }

                    result.Add(new Tuple<string, string>(fieldInfo.ToString(), value));
                }

                #endregion
            }

            if ((memberType & MemberTypes.Property) != 0)
            {
                #region Property

                //获取静态属性
                var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
                if (includeNonPublic)
                {
                    propertyInfos.AddRange(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static));
                }

                //获取实例属性
                if (obj != null)
                {
                    propertyInfos.AddRange(
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                    if (includeNonPublic)
                    {
                        propertyInfos.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic));
                    }
                }

                foreach (var propertyInfo in propertyInfos)
                {
                    string value;
                    try
                    {
                        var propertyValue = propertyInfo.GetValue(obj);
                        value = propertyValue == null ? string.Empty : propertyValue.ToString().Trim().Ellipsis();
                    }
                    catch (Exception)
                    {
                        value = "error";
                    }

                    result.Add(new Tuple<string, string>(propertyInfo.ToString(), value));
                }

                #endregion
            }

            if ((memberType & MemberTypes.Method) != 0)
            {
                #region Method

                //获取静态方法
                var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .ToList();
                if (includeNonPublic)
                {
                    methodInfos.AddRange(type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static));
                }

                //获取实例方法
                if (obj != null)
                {
                    methodInfos.AddRange(type.GetMethods(BindingFlags.Instance | BindingFlags.Public));

                    if (includeNonPublic)
                    {
                        methodInfos.AddRange(type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic));
                    }
                }

                foreach (var methodInfo in methodInfos)
                {
                    //如果没有参数并且有返回值，再求值
                    if (methodInfo.ReturnType != typeof(void))
                    {
                        string value;
                        try
                        {
                            var returnValue = methodInfo.Invoke(obj, null);
                            value = returnValue == null ? string.Empty : returnValue.ToString().Trim().Ellipsis();
                        }
                        catch (Exception)
                        {
                            value = "error";
                        }

                        result.Add(new Tuple<string, string>(methodInfo.ToString(), value));
                    }
                }

                #endregion
            }

            return result;
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
            
            var enumerable = values as Tuple<string, string>[] ?? values.ToArray();
            if (!enumerable.Any())
            {
                return sb;
            }

            var maxKeyLength = enumerable.Max(s => s.Item1.Length);
            var maxValueLength = enumerable.Max(s => s.Item2.Length);

            foreach (var value in enumerable)
            {
                var line = $"|{value.Item1.PadRight(maxKeyLength, ' ')}|{value.Item2.PadRight(maxValueLength, ' ')}|";
                sb.AppendLine(line);
            }

            return sb;
        }
    }
}