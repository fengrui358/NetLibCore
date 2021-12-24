using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        /// 打印对象公共属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string OutputObjectPublicProperty(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var type = obj.GetType();
            var outPutValues = new List<Tuple<string, string>>();
            foreach (var reflectionInfo in GetReflectionInfos(MemberTypes.Property, type, obj, shortName: true))
            {
                outPutValues.Add(reflectionInfo);
            }

            return GetPrintStringBuilder(outPutValues, true).ToString();
        }

        /// <summary>
        /// 打印集合对象公共属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string OutputListPublicProperty<T>(IEnumerable<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var enumerable = list as T[] ?? list.ToArray();
            var f = enumerable.FirstOrDefault();
            if (f == null)
            {
                return string.Empty;
            }

            var type = f.GetType();
            var outPutValues = new List<Tuple<string, string>>();

            foreach (var obj in enumerable)
            {
                foreach (var reflectionInfo in GetReflectionInfos(MemberTypes.Property, type, obj, shortName: true))
                {
                    outPutValues.Add(reflectionInfo);
                }
            }

            return GetPrintStringBuilder(outPutValues, true).ToString();
        }

        /// <summary>
        /// 获取类型的静态相关字段、属性、方法
        /// </summary>
        /// <param name="memberType">要获取的类型</param>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <param name="includeNonPublic">是否包括非公有信息</param>
        /// <param name="shortName">精简属性名、方法名</param>
        /// <param name="fillSpace">优化显示效果，在前后填充空格</param>
        /// <returns>打印信息</returns>
        private static IEnumerable<Tuple<string, string>> GetReflectionInfos(MemberTypes memberType, Type type,
            object obj = null, bool includeNonPublic = false, bool shortName = false, bool fillSpace = true)
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

            // 用于优化显示效果，在前后填充空格
            string FillSpace(string input)
            {
                if (fillSpace)
                {
                    return $" {input} ";
                }

                return input;
            }

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

                    result.Add(new Tuple<string, string>(FillSpace(shortName ? fieldInfo.Name : fieldInfo.ToString()), FillSpace(value)));
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

                    result.Add(new Tuple<string, string>(FillSpace(shortName ? propertyInfo.Name : propertyInfo.ToString()), FillSpace(value)));
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

                        result.Add(new Tuple<string, string>(FillSpace(shortName ? methodInfo.Name : methodInfo.ToString()), FillSpace(value)));
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
        /// <param name="table">使用表格形式打印</param>
        /// <returns>返回打印字符的拼接</returns>
        private static StringBuilder GetPrintStringBuilder(IEnumerable<Tuple<string, string>> values, bool table = false)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var enumerable = values as Tuple<string, string>[] ?? values.ToArray();
            var sb = new StringBuilder();

            if (!enumerable.Any())
            {
                return sb;
            }

            if (!table)
            {
                var maxKeyLength = enumerable.Max(s => GetLength(s.Item1));
                var maxValueLength = enumerable.Max(s => GetLength(s.Item2));

                foreach (var value in enumerable)
                {
                    var line =
                        $"|{value.Item1.PadRight(GetDisplayLength(value.Item1, maxKeyLength), ' ')}|{value.Item2.PadRight(GetDisplayLength(value.Item2, maxValueLength), ' ')}|";
                    sb.AppendLine(line);
                }
            }
            else
            {
                var headers = new List<Tuple<string, int>>();
                var rows = new List<List<string>>();

                foreach (var value in enumerable)
                {
                    var headerTuple = headers.FirstOrDefault(s => s.Item1 == value.Item1);
                    if (headerTuple == null)
                    {
                        headerTuple = new Tuple<string, int>(value.Item1, GetLength(value.Item1));
                        headers.Add(headerTuple);
                    }

                    var valueLength = GetLength(value.Item2);
                    if (headerTuple.Item2 < valueLength)
                    {
                        var index = headers.IndexOf(headerTuple);
                        headers[index] = new Tuple<string, int>(value.Item1, valueLength);
                    }

                    if (value.Item1 == headers[0].Item1)
                    {
                        rows.Add(new List<string>());
                    }

                    var row = rows[rows.Count - 1];
                    row.Add(value.Item2);
                }
                // 打印头
                var headerStr = new StringBuilder();
                var divider = new StringBuilder();
                foreach (var header in headers)
                {
                    var displaylength = GetDisplayLength(header.Item1, header.Item2);
                    var dividerChars = new char[header.Item2];
                    for (var i = 0; i < header.Item2; i++)
                    {
                        dividerChars[i] = '-';
                    }

                    divider.Append($"|{new string(dividerChars)}");
                    var headerDisplayLength = GetDisplayLength(header.Item1, GetLength(header.Item1));
                    var padLeft = (displaylength - headerDisplayLength) / 2;
                    
                    headerStr.Append($"|{header.Item1.PadLeft(padLeft + headerDisplayLength, ' ').PadRight(displaylength, ' ')}");
                }

                headerStr.Append("|");
                divider.Append("|");
                sb.AppendLine(divider.ToString());
                sb.AppendLine(headerStr.ToString());
                sb.AppendLine(divider.ToString());

                // 打印行
                foreach (var row in rows)
                {
                    var rowStr = new StringBuilder();
                    for (var i = 0; i < row.Count; i++)
                    {
                        var header = headers[i];
                        rowStr.Append($"|{row[i].PadRight(GetDisplayLength(row[i], header.Item2), ' ')}");
                    }
                    rowStr.Append("|");
                    sb.AppendLine(rowStr.ToString());
                }

                sb.AppendLine(divider.ToString());
            }

            return sb;
        }

        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static int GetLength(string s)
        {
            var matchs = System.Text.RegularExpressions.Regex.Matches(s, "[\u4e00-\u9fa5]");
            return s.Length + matchs.Count;
        }

        /// <summary>
        /// 获取显示长度
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private static int GetDisplayLength(string s, int maxLength)
        {
            var matchs = System.Text.RegularExpressions.Regex.Matches(s, "[\u4e00-\u9fa5]");
            return maxLength - matchs.Count;
        }
    }
}