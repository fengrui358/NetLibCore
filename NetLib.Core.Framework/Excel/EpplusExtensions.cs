using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FrHello.NetLib.Core.Framework.Excel.Attributes;
using FrHello.NetLib.Core.Reflection;
using OfficeOpenXml;

namespace FrHello.NetLib.Core.Framework
{
    /// <summary>
    /// Excel辅助类
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 最后一行有数据的行号
        /// </summary>
        /// <param name="excelWorksheet">工作表</param>
        /// <returns>最大行号</returns>
        public static int MaxRowNum(this ExcelWorksheet excelWorksheet)
        {
            if (excelWorksheet == null)
            {
                throw new ArgumentNullException(nameof(excelWorksheet));
            }

            return excelWorksheet.Dimension?.End.Row ?? 0;
        }

        /// <summary>
        /// 最后一列有数据的列号
        /// </summary>
        /// <param name="excelWorksheet">工作表</param>
        /// <returns>最大列号</returns>
        public static int MaxColumnNum(this ExcelWorksheet excelWorksheet)
        {
            if (excelWorksheet == null)
            {
                throw new ArgumentNullException(nameof(excelWorksheet));
            }

            return excelWorksheet.Dimension?.End.Column ?? 0;
        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="excelWorksheet">工作表</param>
        /// <param name="headerName">列头名称</param>
        /// <param name="rowNum">行号</param>
        /// <returns>对应单元格</returns>
        public static ExcelRange GetCell(this ExcelWorksheet excelWorksheet, string headerName, int rowNum)
        {
            if (excelWorksheet == null)
            {
                throw new ArgumentNullException(nameof(excelWorksheet));
            }

            if (string.IsNullOrWhiteSpace(headerName))
            {
                return null;
            }

            return excelWorksheet.Cells[rowNum, excelWorksheet.GetColumnNum(headerName)];
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="excelWorksheet">工作表</param>
        /// <param name="headerName">列头名称</param>
        /// <param name="rowNum">行号</param>
        /// <returns>单元格的值</returns>
        public static string GetValue(this ExcelWorksheet excelWorksheet, string headerName, int rowNum)
        {
            return excelWorksheet.GetCell(headerName, rowNum)?.Value?.ToString();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="excelWorksheet">工作表</param>
        /// <param name="headerName">列头名称</param>
        /// <param name="rowNum">行号</param>
        /// <returns>单元格的值</returns>
        public static T GetValue<T>(this ExcelWorksheet excelWorksheet, string headerName, int rowNum)
        {
            var cell = excelWorksheet.GetCell(headerName, rowNum);
            return cell != null ? cell.GetValue<T>() : default;
        }

        /// <summary>
        /// 从Excel中获取特定的类型数据集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="excelPackage"></param>
        /// <returns></returns>
        public static IEnumerable<T> FillDatas<T>(this ExcelPackage excelPackage)
        {
            if (excelPackage == null)
            {
                throw new ArgumentNullException(nameof(excelPackage));
            }

            var type = typeof(T);
            var result = new List<T>();

            string sheetName;
            var sheetAttribute = type.GetCustomAttribute(typeof(SheetAttribute));
            if (sheetAttribute is SheetAttribute sheetAttributeInner)
            {
                sheetName = sheetAttributeInner.SheetName;
            }
            else
            {
                sheetName = nameof(T);
            }

            ExcelWorksheet worksheet = null;

            //判断有无对应的表
            foreach (var workbookWorksheet in excelPackage.Workbook.Worksheets)
            {
                if (workbookWorksheet.Name == sheetName)
                {
                    worksheet = workbookWorksheet;
                    break;
                }
            }

            if (worksheet != null)
            {
                var properties = new Dictionary<string, PropertyInfo>();

                //收集有效的属性
                var allPropertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var propertyInfo in allPropertyInfos)
                {
                    string columnName;
                    var sheetColumnAttribute = propertyInfo.GetCustomAttribute(typeof(SheetColumnAttribute));
                    if (sheetColumnAttribute is SheetColumnAttribute sheetColumnAttributeInner)
                    {
                        columnName = sheetColumnAttributeInner.ColumnName;
                    }
                    else
                    {
                        columnName = propertyInfo.Name;
                    }

                    properties.Add(columnName, propertyInfo);
                }

                //有效的列
                var validColums = new Dictionary<int, PropertyInfo>();

                foreach (var keyValuePair in properties)
                {
                    var column = worksheet.GetColumnNum(keyValuePair.Key);
                    if (column > 0)
                    {
                        validColums.Add(column, keyValuePair.Value);
                    }
                }

                if (validColums.Any())
                {
                    for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.MaxRowNum(); i++)
                    {
                        var instance = Activator.CreateInstance<T>();
                        foreach (var keyValuePair in validColums)
                        {
                            var value = TypeHelper.ChangeType(worksheet.Cells[i, keyValuePair.Key].Value,
                                keyValuePair.Value.PropertyType);

                            keyValuePair.Value.SetValue(instance, value);
                        }

                        result.Add(instance);
                    }
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 根据列头名称获取列号
        /// </summary>
        /// <param name="excelWorksheet">工作表</param>
        /// <param name="headerName">列头名称</param>
        /// <returns>列号</returns>
        private static int GetColumnNum(this ExcelWorksheet excelWorksheet, string headerName)
        {
            if (excelWorksheet == null)
            {
                throw new ArgumentNullException(nameof(excelWorksheet));
            }

            if (string.IsNullOrWhiteSpace(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            for (int i = 1; i <= excelWorksheet.MaxColumnNum(); i++)
            {
                var header = excelWorksheet.Cells[excelWorksheet.Dimension.Start.Row, i].Value;
                if (header != null && header.ToString() == headerName)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
