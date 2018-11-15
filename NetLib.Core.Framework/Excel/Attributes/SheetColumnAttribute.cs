using System;

namespace FrHello.NetLib.Core.Framework.Excel.Attributes
{
    /// <summary>
    /// 申明列的名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SheetColumnAttribute : Attribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="columnName">列的名称</param>
        public SheetColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// 列的名称
        /// </summary>
        public string ColumnName { get; }
    }
}
