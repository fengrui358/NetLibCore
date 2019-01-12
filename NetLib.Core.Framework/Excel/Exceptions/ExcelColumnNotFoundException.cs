using System;
using System.IO;

namespace FrHello.NetLib.Core.Framework.Excel.Exceptions
{
    /// <summary>
    /// Excel对应的列没有找到
    /// </summary>
    public class ExcelColumnNotFoundException : Exception
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// 对应的文件
        /// </summary>
        public FileInfo ExcelFileInfo { get; }

        /// <summary>
        /// 表名
        /// </summary>
        public string SheetName { get; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public override string Message => $"ColumnName: {ColumnName} not found.";

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="columnName">Excel列号</param>
        /// <param name="excelFileInfo">Excel文件</param>
        /// <param name="sheetName">Excel表名</param>
        public ExcelColumnNotFoundException(string columnName, FileInfo excelFileInfo, string sheetName)
        {
            ColumnName = columnName;
            ExcelFileInfo = excelFileInfo;
            SheetName = sheetName;
        }
    }
}
