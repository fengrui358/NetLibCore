using System;
using System.IO;

namespace FrHello.NetLib.Core.Framework.Excel.Exceptions
{
    /// <summary>
    /// Excel单元格不为空异常
    /// </summary>
    public class ExcelCellNotAllowNullException : Exception
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; }

        /// <summary>
        /// 列号
        /// </summary>
        public int ColumnNum { get; }

        /// <summary>
        /// 表名
        /// </summary>
        public string SheetName { get; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public override string Message => $"Row:{RowNum} Column:{ColumnNum} not allow null.";

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="rowNum">行号</param>
        /// <param name="columnNum">列号</param>
        /// <param name="sheetName">Excel表名</param>
        public ExcelCellNotAllowNullException(int rowNum, int columnNum, string sheetName)
        {
            RowNum = rowNum;
            ColumnNum = columnNum;
            SheetName = sheetName;
        }
    }
}