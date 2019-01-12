using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FrHello.NetLib.Core.Framework.Excel.Attributes;
using FrHello.NetLib.Core.Framework.Excel.Exceptions;
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
        /// <param name="excelPackage">工作表</param>
        /// <returns>填充后的集合对象</returns>
        public static IEnumerable<T> FillDatas<T>(this ExcelPackage excelPackage)
        {
            if (excelPackage == null)
            {
                throw new ArgumentNullException(nameof(excelPackage));
            }

            if (excelPackage.File != null && !excelPackage.File.Exists)
            {
                throw new FileNotFoundException($"{excelPackage.File.FullName} not found.");
            }

            var type = typeof(T);

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
                var properties = new List<ColumnDescription>();

                //收集有效的属性
                var allPropertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var propertyInfo in allPropertyInfos)
                {
                    string columnName;
                    var sheetColumnAttribute = propertyInfo.GetCustomAttribute(typeof(SheetColumnAttribute));
                    if (sheetColumnAttribute is SheetColumnAttribute sheetColumnAttributeInner)
                    {
                        columnName = sheetColumnAttributeInner.ColumnName;
                        var allowNull = sheetColumnAttributeInner.AllowNull;

                        properties.Add(new ColumnDescription(columnName, propertyInfo, true) {AllowNull = allowNull});
                    }
                    else
                    {
                        columnName = propertyInfo.Name;
                        properties.Add(new ColumnDescription(columnName, propertyInfo));
                    }
                }

                //有效的列
                var validColums = new List<ColumnDescription>();

                foreach (var property in properties)
                {
                    //添加了明确列名找不到的情况需要提示

                    var column = worksheet.GetColumnNum(property.ColumnName);
                    if (column > 0)
                    {
                        property.ColumnNum = column;
                        validColums.Add(property);
                    }
                    else if(property.IsNeed)
                    {
                        //如果必须要该列又不存在则抛出异常
                        var excelColumnNotFoundException = new ExcelColumnNotFoundException(property.ColumnName, excelPackage.File,
                            worksheet.Name);
                        throw excelColumnNotFoundException;
                    }
                }

                if (validColums.Any())
                {
                    for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.MaxRowNum(); i++)
                    {
                        var instance = Activator.CreateInstance<T>();
                        foreach (var columnDescription in validColums)
                        {
                            object value;

                            var converterAttribute = columnDescription.PropertyInfo.GetCustomAttribute<SheetColumnValueConverterAttribute>();
                            if (converterAttribute != null)
                            {
                                value = converterAttribute.SimpleValueConverter.Convert(worksheet
                                    .Cells[i, columnDescription.ColumnNum].Value);
                            }
                            else
                            {
                                try
                                {
                                    value = TypeHelper.ChangeType(worksheet.Cells[i, columnDescription.ColumnNum].Value,
                                        columnDescription.PropertyInfo.PropertyType);
                                }
                                catch (Exception e)
                                {
                                    throw new ExcelCellChangeTypeException(i, columnDescription.ColumnNum,
                                        excelPackage.File, worksheet.Name, e);
                                }
                            }

                            if (value == null || (value is string valueStr && string.IsNullOrWhiteSpace(valueStr)))
                            {
                                //判断是否有空特性
                                if (!columnDescription.AllowNull)
                                {
                                    throw new ExcelCellNotAllowNullException(i, columnDescription.ColumnNum,
                                        excelPackage.File, worksheet.Name);
                                }
                            }

                            columnDescription.PropertyInfo.SetValue(instance, value);
                        }

                        yield return instance;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Sheet name:{sheetName} not found any columns to convert.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Sheet name: {sheetName} not found.");
            }
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

        /// <summary>
        /// 列描述
        /// </summary>
        private class ColumnDescription
        {
            public string ColumnName { get; }

            public int ColumnNum { get; set; }

            public bool AllowNull { get; set; } = true;

            public PropertyInfo PropertyInfo { get; }

            /// <summary>
            /// 是否是必须的列
            /// </summary>
            public bool IsNeed { get; }

            public ColumnDescription(string columnName, PropertyInfo propertyInfo, bool isNeed = false)
            {
                ColumnName = columnName;
                PropertyInfo = propertyInfo;

                IsNeed = isNeed;
            }
        }
    }
}
