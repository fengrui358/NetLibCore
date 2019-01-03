using System;
using FrHello.NetLib.Core.Interfaces;

namespace FrHello.NetLib.Core.Framework.Excel.Attributes
{
    /// <summary>
    /// 申明列的值要进行值转换
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SheetColumnValueConverterAttribute : Attribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="simpleValueConverterType">需要继承ISimpleValueConverter接口</param>
        public SheetColumnValueConverterAttribute(Type simpleValueConverterType)
        {
            if (simpleValueConverterType == null)
            {
                throw new ArgumentNullException(nameof(simpleValueConverterType));
            }

            if (typeof(ISimpleValueConverter).IsAssignableFrom(simpleValueConverterType))
            {
                SimpleValueConverter = (ISimpleValueConverter) Activator.CreateInstance(simpleValueConverterType);
            }
            else
            {
                throw new InvalidOperationException(nameof(simpleValueConverterType));
            }
        }

        /// <summary>
        /// 值转换器
        /// </summary>
        public ISimpleValueConverter SimpleValueConverter { get; }
    }
}
