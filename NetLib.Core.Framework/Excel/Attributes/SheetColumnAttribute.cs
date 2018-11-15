using System;

namespace FrHello.NetLib.Core.Framework.Excel.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SheetColumnAttribute : Attribute
    {
        public SheetColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; }
    }
}
