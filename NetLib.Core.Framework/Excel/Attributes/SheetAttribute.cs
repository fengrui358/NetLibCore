using System;
using System.Collections.Generic;
using System.Text;

namespace FrHello.NetLib.Core.Framework.Excel.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SheetAttribute : Attribute
    {
        public SheetAttribute(string sheetName)
        {
            SheetName = sheetName;
        }

        public string SheetName { get; }
    }
}
