using System;

namespace FrHello.NetLib.Core.Framework.Excel.Attributes
{
    /// <summary>
    /// 指定某个属性是行号，需要填充
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RowNumAttribute : Attribute
    {
    }
}
