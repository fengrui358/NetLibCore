using System;

namespace FrHello.NetLib.Core.Framework.Excel.Attributes
{
    /// <summary>
    /// 申明表的名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SheetAttribute : Attribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="sheetName">表的名称</param>
        public SheetAttribute(string sheetName)
        {
            SheetName = sheetName;
        }

        /// <summary>
        /// 表的名称
        /// </summary>
        public string SheetName { get; }
    }
}