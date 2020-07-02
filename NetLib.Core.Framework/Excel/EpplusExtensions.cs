using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FrHello.NetLib.Core.Attributes;
using FrHello.NetLib.Core.Framework.Excel;
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
        static Extensions()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

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
            var sheetName = GetSheetName(type);

            //判断有无对应的表
            var worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName);

            if (worksheet != null)
            {
                var startRow = worksheet.Dimension.Start.Row + 1;
                return FillDatas<T>(excelPackage, startRow, worksheet.MaxRowNum());
            }
            else
            {
                throw new InvalidOperationException($"Sheet name: {sheetName} not found.");
            }
        }

        /// <summary>
        /// 从Excel中获取特定的类型数据集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="excelPackage">工作表</param>
        /// <param name="rowFrom">起始行</param>
        /// <param name="count">读取数据</param>
        /// <returns>填充后的集合对象</returns>
        public static IEnumerable<T> FillDatas<T>(this ExcelPackage excelPackage, int rowFrom, int count)
        {
            if (excelPackage == null)
            {
                throw new ArgumentNullException(nameof(excelPackage));
            }

            if (excelPackage.File != null && !excelPackage.File.Exists)
            {
                throw new FileNotFoundException($"{excelPackage.File.FullName} not found.");
            }

            if (rowFrom < 1)
            {
                throw new ArgumentException($"{nameof(rowFrom)}:{rowFrom}");
            }

            if (rowFrom == 1)
            {
                //不能插在列头前
                rowFrom = 2;
            }

            var type = typeof(T);
            var sheetName = GetSheetName(type);

            //判断有无对应的表
            var worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName);

            if (worksheet != null)
            {
                return worksheet.FillDatas<T>(rowFrom, count);
            }
            else
            {
                throw new InvalidOperationException($"Sheet name: {sheetName} not found.");
            }
        }

        /// <summary>
        /// 从Excel指定表中获取特定的类型数据集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="worksheet">工作表</param>
        /// <param name="rowFrom">起始行</param>
        /// <param name="count">读取数据</param>
        /// <returns>填充后的集合对象</returns>
        public static IEnumerable<T> FillDatas<T>(this ExcelWorksheet worksheet, int rowFrom, int count)
        {
            //收集有效的属性
            var properties = ConvertProperitesToColumnDescription(typeof(T));

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
                else if (property.IsNeed)
                {
                    //如果必须要该列又不存在则抛出异常
                    var excelColumnNotFoundException = new ExcelColumnNotFoundException(property.ColumnName,
                        worksheet.Name);
                    throw excelColumnNotFoundException;
                }
            }

            if (validColums.Any())
            {
                var rowNumProperty = GetRowNumPropertyInfo(typeof(T));

                var maxRow = worksheet.MaxRowNum();
                maxRow = Math.Min(maxRow, rowFrom + count - 1);

                var startRow = Math.Max(rowFrom, worksheet.Dimension.Start.Row + 1);

                for (var i = startRow; i <= maxRow; i++)
                {
                    var instance = Activator.CreateInstance<T>();
                    foreach (var columnDescription in validColums)
                    {
                        object value;

                        var converterAttribute = columnDescription.PropertyInfo
                            .GetCustomAttribute<SheetColumnValueConverterAttribute>();
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
                                    worksheet.Name, e);
                            }
                        }

                        if (value == null || (value is string valueStr && string.IsNullOrWhiteSpace(valueStr)))
                        {
                            //判断是否有空特性
                            if (!columnDescription.AllowNull)
                            {
                                throw new ExcelCellNotAllowNullException(i, columnDescription.ColumnNum,
                                    worksheet.Name);
                            }
                        }

                        columnDescription.PropertyInfo.SetValue(instance, value);
                    }

                    if (rowNumProperty != null)
                    {
                        rowNumProperty.SetValue(instance, i);
                    }

                    yield return instance;
                }
            }
            else
            {
                throw new InvalidOperationException($"Sheet name:{worksheet.Name} not found any columns to convert.");
            }
        }

        /// <summary>
        /// 将数据集合写入Excel文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="rowDatas">行数据</param>
        /// <param name="excelPath">excel文件路径</param>
        /// <param name="overWrite">覆盖写入还是追加写入</param>
        public static async Task WriteDatas<T>(IEnumerable<T> rowDatas, string excelPath, bool overWrite = false)
        {
            if (rowDatas == null)
            {
                throw new ArgumentNullException(nameof(rowDatas));
            }

            if (string.IsNullOrWhiteSpace(excelPath))
            {
                throw new ArgumentNullException(nameof(excelPath));
            }

            if (overWrite && File.Exists(excelPath))
            {
                File.Delete(excelPath);
            }

            using var excelPackage = new ExcelPackage(new FileInfo(excelPath));
            var workSheetName = GetSheetName(typeof(T));

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == workSheetName) ??
                            excelPackage.Workbook.Worksheets.Add(workSheetName);

            var maxRowNum = workSheet.MaxRowNum() + 1;

            await InsertDatas(rowDatas, excelPath, maxRowNum);
        }

        /// <summary>
        /// 向Excel插入数据集
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="rowDatas">行数据</param>
        /// <param name="excelPath">excel文件路径</param>
        /// <param name="rowFrom">起始行</param>
        /// <returns></returns>
        public static async Task InsertDatas<T>(IEnumerable<T> rowDatas, string excelPath, int rowFrom)
        {
            if (rowDatas == null)
            {
                throw new ArgumentNullException(nameof(rowDatas));
            }

            if (string.IsNullOrWhiteSpace(excelPath))
            {
                throw new ArgumentNullException(nameof(excelPath));
            }

            if (rowFrom < 1)
            {
                throw new ArgumentException($"{nameof(rowFrom)}:{rowFrom}");
            }

            var rows = rowDatas as T[] ?? rowDatas.ToArray();
            if (!rows.Any())
            {
                return;
            }

            if (rowFrom == 1)
            {
                //不能插在列头前
                rowFrom = 2;
            }

            using var excelPackage = new ExcelPackage(new FileInfo(excelPath));
            var workSheetName = GetSheetName(typeof(T));

            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == workSheetName) ??
                            excelPackage.Workbook.Worksheets.Add(workSheetName);

            //收集有效的属性
            var properties = ConvertProperitesToColumnDescription(typeof(T)).ToList();

            var maxColumnNum = workSheet.MaxColumnNum() + 1;
            foreach (var columnDescription in properties)
            {
                var columnNum = workSheet.GetColumnNum(columnDescription.ColumnName);

                if (columnNum == 0)
                {
                    workSheet.Cells[1, maxColumnNum].Value = columnDescription.ColumnName;
                    columnDescription.ColumnNum = maxColumnNum;
                    maxColumnNum++;
                }
                else
                {
                    columnDescription.ColumnNum = columnNum;
                }
            }

            workSheet.InsertRow(rowFrom, rows.Length, rowFrom);
            var rowNumProperty = GetRowNumPropertyInfo(typeof(T));

            foreach (var rowData in rows)
            {
                var writeSomething = false;
                foreach (var columnDescription in properties)
                {
                    var value = columnDescription.PropertyInfo.GetValue(rowData);
                    if (value != null)
                    {
                        workSheet.Cells[rowFrom, columnDescription.ColumnNum].Value =
                            TypeHelper.ChangeType(value, columnDescription.PropertyInfo.PropertyType);
                        writeSomething = true;

                        if (rowNumProperty != null && rowNumProperty.CanWrite)
                        {
                            rowNumProperty.SetValue(rowData, rowFrom);
                        }
                    }
                }

                if (writeSomething)
                {
                    rowFrom++;
                }
            }

            await excelPackage.SaveAsync();
        }

        /// <summary>
        /// 向Excel中添加行数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="excelPath">excel文件路径</param>
        /// <param name="rowData">行数据</param>
        public static async Task AppendRow<T>(string excelPath, T rowData)
        {
            if (string.IsNullOrEmpty(excelPath))
            {
                throw new ArgumentNullException(nameof(excelPath));
            }

            using var excelPackage = new ExcelPackage(new FileInfo(excelPath));
            await AppendRow(excelPackage, rowData);
        }

        /// <summary>
        /// 向Excel中添加行数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="excelPackage">excel文件</param>
        /// <param name="rowData">行数据</param>
        public static async Task AppendRow<T>(this ExcelPackage excelPackage, T rowData)
        {
            if (excelPackage == null)
            {
                throw new ArgumentNullException(nameof(excelPackage));
            }

            if (rowData == null)
            {
                return;
            }

            var workSheetName = GetSheetName(typeof(T));
            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == workSheetName) ??
                            excelPackage.Workbook.Worksheets.Add(workSheetName);

            var maxRowNum = workSheet.MaxRowNum() + 1;

            await InsertRow(excelPackage, maxRowNum, rowData);
        }

        /// <summary>
        /// 向Excel中插入行数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="excelPackage">excel文件</param>
        /// <param name="rowFrom">起始行</param>
        /// <param name="rowData">行数据</param>
        /// <returns></returns>
        public static async Task InsertRow<T>(this ExcelPackage excelPackage, int rowFrom, T rowData)
        {
            if (excelPackage == null)
            {
                throw new ArgumentNullException(nameof(excelPackage));
            }

            if (rowFrom < 1)
            {
                throw new ArgumentException($"{nameof(rowFrom)}:{rowFrom}");
            }

            if (rowData == null)
            {
                return;
            }

            if (rowFrom == 1)
            {
                //不能插在列头前
                rowFrom = 2;
            }

            var workSheetName = GetSheetName(typeof(T));
            var workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == workSheetName) ??
                            excelPackage.Workbook.Worksheets.Add(workSheetName);

            //收集有效的属性
            var properties = ConvertProperitesToColumnDescription(typeof(T)).ToList();

            var maxColumnNum = workSheet.MaxColumnNum() + 1;
            foreach (var columnDescription in properties)
            {
                var columnNum = workSheet.GetColumnNum(columnDescription.ColumnName);

                if (columnNum == 0)
                {
                    workSheet.Cells[1, maxColumnNum].Value = columnDescription.ColumnName;
                    columnDescription.ColumnNum = maxColumnNum;
                    maxColumnNum++;
                }
                else
                {
                    columnDescription.ColumnNum = columnNum;
                }
            }

            workSheet.InsertRow(rowFrom, 1, rowFrom);

            var rowNumProperty = GetRowNumPropertyInfo(typeof(T));
            foreach (var columnDescription in properties)
            {
                var value = columnDescription.PropertyInfo.GetValue(rowData);
                if (value != null)
                {
                    workSheet.Cells[rowFrom, columnDescription.ColumnNum].Value =
                        TypeHelper.ChangeType(value, columnDescription.PropertyInfo.PropertyType);

                    if (rowNumProperty != null)
                    {
                        rowNumProperty.SetValue(rowData, rowFrom);
                    }
                }
            }

            await excelPackage.SaveAsync();
        }

        /// <summary>
        /// 根据列头名称获取列号
        /// </summary>
        /// <param name="excelWorksheet">工作表</param>
        /// <param name="headerName">列头名称</param>
        /// <returns>列号（返回0表示未找到对应列名）</returns>
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
        /// 获取列标题
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        private static string GetSheetName(MemberInfo type)
        {
            string sheetName;
            var sheetAttribute = type.GetCustomAttribute(typeof(SheetAttribute));
            if (sheetAttribute is SheetAttribute sheetAttributeInner)
            {
                sheetName = ExcelHelper.ReplaceInvalidSheetName(sheetAttributeInner.SheetName);
            }
            else
            {
                sheetName = ExcelHelper.ReplaceInvalidSheetName(type.Name);
            }

            return sheetName;
        }

        /// <summary>
        /// 转换类型信息为表列信息
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        private static IEnumerable<ColumnDescription> ConvertProperitesToColumnDescription(Type type)
        {
            //收集有效的属性
            var allPropertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in allPropertyInfos)
            {
                if (propertyInfo.GetCustomAttribute(typeof(IgnoreAttribute)) != null ||
                    propertyInfo.GetCustomAttribute(typeof(RowNumAttribute)) != null)
                {
                    continue;
                }

                string columnName;
                var sheetColumnAttribute = propertyInfo.GetCustomAttribute(typeof(SheetColumnAttribute));
                if (sheetColumnAttribute is SheetColumnAttribute sheetColumnAttributeInner)
                {
                    columnName = sheetColumnAttributeInner.ColumnName;
                    var allowNull = sheetColumnAttributeInner.AllowNull;

                    yield return new ColumnDescription(columnName, propertyInfo, true) {AllowNull = allowNull};
                }
                else
                {
                    columnName = propertyInfo.Name;
                    yield return new ColumnDescription(columnName, propertyInfo);
                }
            }
        }

        /// <summary>
        /// 获取行号属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static PropertyInfo GetRowNumPropertyInfo(Type type)
        {
            var allPropertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in allPropertyInfos)
            {
                var rowNumAttribute = propertyInfo.GetCustomAttribute<RowNumAttribute>();
                if (rowNumAttribute != null)
                {
                    return propertyInfo;
                }
            }

            return null;
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