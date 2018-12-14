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

        public override string Message => $"ColumnName: {ColumnName} not found.";

        public ExcelColumnNotFoundException(string columnName, FileInfo excelFileInfo, string sheetName)
        {
            ColumnName = columnName;
            ExcelFileInfo = excelFileInfo;
            SheetName = sheetName;
        }
    }
}
