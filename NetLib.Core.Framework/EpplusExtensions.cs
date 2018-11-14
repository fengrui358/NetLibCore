using System;
using OfficeOpenXml;

namespace FrHello.NetLib.Core.Framework
{
    public static class Extensions
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
