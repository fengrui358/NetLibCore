using System;
using System.IO;

namespace FrHello.NetLib.Core.Framework.Excel.Exceptions
{
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
        /// 对应的文件
        /// </summary>
        public FileInfo ExcelFileInfo { get; }

        /// <summary>
        /// 表名
        /// </summary>
        public string SheetName { get; }

        public override string Message => $"Row:{RowNum} Column:{ColumnNum} not allow null.";

        public ExcelCellNotAllowNullException(int rowNum, int columnNum, FileInfo excelFileInfo, string sheetName)
        {
            RowNum = rowNum;
            ColumnNum = columnNum;
            ExcelFileInfo = excelFileInfo;
            SheetName = sheetName;
        }
    }
}