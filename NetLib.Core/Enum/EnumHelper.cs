using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrHello.NetLib.Core.Enum
{
    /// <summary>
    /// 枚举辅助方法
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 将多个同类型的枚举合并为一个
        /// </summary>
        /// <param name="flagsEnum">待合并的枚举</param>
        /// <param name="enums">将要合并进来的枚举</param>
        /// <returns>合并之后的枚举值</returns>
        public static System.Enum Combine(this System.Enum flagsEnum, params System.Enum[] enums)
        {
            return flagsEnum.Combine(true, enums);
        }

        /// <summary>
        /// 将多个同类型的枚举合并为一个
        /// </summary>
        /// <param name="flagsEnum">待合并的枚举</param>
        /// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        /// <param name="enums">将要合并进来的枚举</param>
        /// <returns>合并之后的枚举值</returns>
        public static System.Enum Combine(this System.Enum flagsEnum, bool verifyFlagsAttribute, params System.Enum[] enums)
        {
            if (flagsEnum == null)
            {
                throw new ArgumentNullException(nameof(flagsEnum));
            }

            var enumsLength = enums?.Length ?? 0;

            var newEnums = new System.Enum[enumsLength + 1];
            newEnums[0] = flagsEnum;
            enums?.CopyTo(newEnums, 1);
            return Combine(verifyFlagsAttribute, newEnums);
        }

        /// <summary>
        /// 将多个同类型的枚举合并为一个
        /// </summary>
        /// <param name="enums">将要合并进来的枚举</param>
        /// <returns>合并之后的枚举值</returns>
        public static System.Enum Combine(params System.Enum[] enums)
        {
            return Combine(true, enums);
        }

        /// <summary>
        /// 将多个同类型的枚举合并为一个
        /// </summary>
        /// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        /// <param name="enums">将要合并进来的枚举</param>
        /// <returns>合并之后的枚举值</returns>
        public static System.Enum Combine(bool verifyFlagsAttribute, params System.Enum[] enums)
        {
            if (!IsSameEnumDefine(enums))
            {
                //判断枚举类型是否一致，不一致则直接失败
                throw new ArgumentException($"param {nameof(enums)} inconsistent parameter type.");
            }

            //校验flags特性标签
            if (verifyFlagsAttribute && !IsDefineFlagsAttribute(enums.FirstOrDefault()))
            {
                //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
                throw new ArgumentException($"param {nameof(enums)} is not contains flags attribute.");
            }

            System.Enum resultEnum = null;
            //拼装
            foreach (var @enum in enums)
            {
                if (resultEnum == null)
                {
                    resultEnum = @enum;
                }
                else
                {
                    //todo:整形转换回枚举
                    var intresultEnum = Convert.ToInt32(resultEnum) | Convert.ToInt32(@enum);
                }
            }

            return null;
        }

        /// <summary>
        /// 校验某个枚举组合值里是否包含特定的枚举
        /// </summary>
        /// <param name="combineEnum">结合的枚举</param>
        /// <param name="verifyFlagsAttribute"></param>
        /// <param name="enums"></param>
        /// <returns>是否包含指定的枚举值</returns>
        public static bool Contains(this System.Enum combineEnum, bool verifyFlagsAttribute = true,
            params System.Enum[] enums)
        {
            if (combineEnum == null)
            {
                throw new ArgumentNullException(nameof(combineEnum));
            }

            if (!IsSameEnumDefine(enums))
            {
                //判断枚举类型是否一致，不一致则直接失败
                return false;
            }

            if (verifyFlagsAttribute && !IsDefineFlagsAttribute(enums.FirstOrDefault()))
            {
                //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
                return false;
            }
            

            return false;
        }

        /// <summary>
        /// 是否来源于相同的枚举定义
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static bool IsSameEnumDefine(params System.Enum[] enums)
        {
            Type enumType = null;
            foreach (var @enum in enums)
            {
                if (enumType == null)
                {
                    enumType = @enum.GetType();
                }
                else if(@enum.GetType() != enumType)
                {
                    return false;
                }
            }

            return enumType != null;
        }

        /// <summary>
        /// 是否定义了Flags特性
        /// </summary>
        /// <param name="enum">需要判断的枚举类型</param>
        /// <returns></returns>
        private static bool IsDefineFlagsAttribute(System.Enum @enum)
        {
            if (@enum == null)
            {
                return false;
            }

            var enumType = @enum.GetType();
            return enumType.IsDefined(typeof(FlagsAttribute), false);
        }
    }
}
