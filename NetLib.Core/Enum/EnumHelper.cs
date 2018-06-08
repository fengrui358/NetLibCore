using System;
using System.Linq;

namespace FrHello.NetLib.Core.Enum
{
    /// <summary>
    /// 枚举辅助方法
    /// </summary>
    public static class EnumHelper
    {
        #region Combine

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

        ///// <summary>
        ///// 将多个同类型的枚举合并为一个
        ///// </summary>
        ///// <param name="flagsEnum">待合并的枚举</param>
        ///// <param name="enums">将要合并进来的枚举</param>
        ///// <returns>合并之后的枚举值</returns>
        //public static T Combine<T>(this T flagsEnum, params T[] enums) where T : System.Enum
        //{
        //    return flagsEnum.Combine(true, enums);
        //}

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

        ///// <summary>
        ///// 将多个同类型的枚举合并为一个
        ///// </summary>
        ///// <param name="flagsEnum">待合并的枚举</param>
        ///// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        ///// <param name="enums">将要合并进来的枚举</param>
        ///// <returns>合并之后的枚举值</returns>
        //public static T Combine<T>(this T flagsEnum, bool verifyFlagsAttribute, params T[] enums) where T : System.Enum
        //{
        //    if (flagsEnum == null)
        //    {
        //        throw new ArgumentNullException(nameof(flagsEnum));
        //    }

        //    var enumsLength = enums?.Length ?? 0;

        //    var newEnums = new T[enumsLength + 1];
        //    newEnums[0] = flagsEnum;
        //    enums?.CopyTo(newEnums, 1);
        //    return Combine<T>(verifyFlagsAttribute, newEnums);
        //}

        /// <summary>
        /// 将多个同类型的枚举合并为一个
        /// </summary>
        /// <param name="enums">将要合并进来的枚举</param>
        /// <returns>合并之后的枚举值</returns>
        public static System.Enum Combine(params System.Enum[] enums)
        {
            return Combine(true, enums);
        }

        ///// <summary>
        ///// 将多个同类型的枚举合并为一个
        ///// </summary>
        ///// <param name="enums">将要合并进来的枚举</param>
        ///// <returns>合并之后的枚举值</returns>
        //public static T Combine<T>(params T[] enums) where T : System.Enum
        //{
        //    return Combine(true, enums);
        //}

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
            if (verifyFlagsAttribute && enums.Any() && !IsDefineFlagsAttribute(enums.First()))
            {
                //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
                throw new ArgumentException($"param {nameof(enums)} is not contains flags attribute.");
            }

            System.Enum resultEnum = null;
            var enumType = enums.FirstOrDefault()?.GetType();

            //拼装
            foreach (var @enum in enums)
            {
                if (resultEnum == null)
                {
                    resultEnum = @enum;
                }
                else
                {
                    var intresultEnum = Convert.ToInt32(resultEnum) | Convert.ToInt32(@enum);
                    if (enumType != null)
                    {
                        resultEnum = (System.Enum) System.Enum.ToObject(enumType, intresultEnum);
                    }
                }
            }

            return resultEnum;
        }

        ///// <summary>
        ///// 将多个同类型的枚举合并为一个
        ///// </summary>
        ///// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        ///// <param name="enums">将要合并进来的枚举</param>
        ///// <returns>合并之后的枚举值</returns>
        //public static T Combine<T>(bool verifyFlagsAttribute, params T[] enums) where T : System.Enum
        //{
        //    //校验flags特性标签
        //    if (verifyFlagsAttribute && !IsDefineFlagsAttribute<T>())
        //    {
        //        //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
        //        throw new ArgumentException($"type {typeof(T).Name} is not contains flags attribute.");
        //    }

        //    if (enums == null || !enums.Any())
        //    {
        //        throw new ArgumentException($"param {nameof(enums)} is null or empty.");
        //    }

        //    var resultEnum = enums.FirstOrDefault();

        //    if (enums.Length > 1)
        //    {
        //        for (int i = 1; i < enums.Length; i++)
        //        {
        //            var intresultEnum = Convert.ToInt32(resultEnum) | Convert.ToInt32(enums[i]);
        //            resultEnum = (T) System.Enum.ToObject(typeof(T), intresultEnum);
        //        }
        //    }

        //    return resultEnum;
        //}

        #endregion

        #region Contains

        /// <summary>
        /// 校验某个枚举组合值里是否包含特定的枚举
        /// </summary>
        /// <param name="combinedEnum">结合的枚举</param>
        /// <param name="enums">要判断的枚举值</param>
        /// <returns>是否包含指定的枚举值</returns>
        public static bool Contains(this System.Enum combinedEnum, params System.Enum[] enums)
        {
            return combinedEnum.Contains(true, enums);
        }

        ///// <summary>
        ///// 校验某个枚举组合值里是否包含特定的枚举
        ///// </summary>
        ///// <param name="combinedEnum">结合的枚举</param>
        ///// <param name="enums">要判断的枚举值</param>
        ///// <returns>是否包含指定的枚举值</returns>
        //public static bool Contains<T>(this T combinedEnum, params T[] enums) where T : System.Enum
        //{
        //    return combinedEnum.Contains(true, enums);
        //}

        /// <summary>
        /// 校验某个枚举组合值里是否包含特定的枚举
        /// </summary>
        /// <param name="combinedEnum">结合的枚举</param>
        /// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        /// <param name="enums">要判断的枚举值</param>
        /// <returns>是否包含指定的枚举值</returns>
        public static bool Contains(this System.Enum combinedEnum, bool verifyFlagsAttribute,
            params System.Enum[] enums)
        {
            if (combinedEnum == null)
            {
                throw new ArgumentNullException(nameof(combinedEnum));
            }

            if (enums == null || !enums.Any())
            {
                return false;
            }

            if (!IsSameEnumDefine(enums))
            {
                //判断枚举类型是否一致，不一致则直接失败
                throw new ArgumentException($"param {nameof(enums)} inconsistent parameter type.");
            }

            var isFlag = IsDefineFlagsAttribute(enums.FirstOrDefault());
            if (verifyFlagsAttribute && !isFlag)
            {
                //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
                throw new ArgumentException($"param {nameof(enums)} is not contains flags attribute.");
            }

            var combinedEnumInt = Convert.ToInt32(combinedEnum);
            foreach (var @enum in enums)
            {
                if (isFlag)
                {
                    if (!combinedEnum.HasFlag(@enum))
                    {
                        return false;
                    }
                }
                else
                {
                    if ((combinedEnumInt & Convert.ToInt32(@enum)) == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        ///// <summary>
        ///// 校验某个枚举组合值里是否包含特定的枚举
        ///// </summary>
        ///// <param name="combinedEnum">结合的枚举</param>
        ///// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        ///// <param name="enums">要判断的枚举值</param>
        ///// <returns>是否包含指定的枚举值</returns>
        //public static bool Contains<T>(this T combinedEnum, bool verifyFlagsAttribute, params T[] enums) where T : System.Enum
        //{
        //    if (combinedEnum == null)
        //    {
        //        throw new ArgumentNullException(nameof(combinedEnum));
        //    }

        //    if (enums == null || !enums.Any())
        //    {
        //        return false;
        //    }

        //    var isFlag = IsDefineFlagsAttribute<T>();
        //    if (verifyFlagsAttribute && !isFlag)
        //    {
        //        //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
        //        throw new ArgumentException($"type {typeof(T).Name} is not contains flags attribute.");
        //    }

        //    var combinedEnumInt = Convert.ToInt32(combinedEnum);
        //    foreach (var @enum in enums)
        //    {
        //        if (isFlag)
        //        {
        //            if (!combinedEnum.HasFlag(@enum))
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            if ((combinedEnumInt & Convert.ToInt32(@enum)) == 0)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        #endregion

        #region Remove

        /// <summary>
        /// 从一个组合枚举中移除某一个枚举项
        /// </summary>
        /// <param name="combinedEnum">结合的枚举</param>
        /// <param name="enums">要移除的枚举值</param>
        public static System.Enum Remove(this System.Enum combinedEnum, params System.Enum[] enums)
        {
            return combinedEnum.Remove(enums);
        }

        ///// <summary>
        ///// 从一个组合枚举中移除某一个枚举项
        ///// </summary>
        ///// <param name="combinedEnum">结合的枚举</param>
        ///// <param name="verifyFlagsAttribute">是否要校验枚举是否包含Flags特性</param>
        ///// <param name="enums">要移除的枚举值</param>
        //public static T Remove<T>(this T combinedEnum, bool verifyFlagsAttribute, params T[] enums) where T : System.Enum
        //{
        //    if (combinedEnum == null)
        //    {
        //        throw new ArgumentNullException(nameof(combinedEnum));
        //    }

        //    if (verifyFlagsAttribute && !IsDefineFlagsAttribute<T>())
        //    {
        //        //如果需要校验Flags特性而枚举又没有定义该特性则直接失败
        //        throw new ArgumentException($"type {typeof(T).Name} is not contains flags attribute.");
        //    }

        //    var combinedEnumInt = Convert.ToInt32(combinedEnum);
        //    foreach (var @enum in enums)
        //    {
        //        combinedEnumInt = combinedEnumInt & ~Convert.ToInt32(@enum);
        //    }

        //    return (T) System.Enum.ToObject(typeof(T), combinedEnumInt);
        //}

        #endregion

        #region Private Methods

        /// <summary>
        /// 是否来源于相同的枚举定义
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        private static bool IsSameEnumDefine(params System.Enum[] enums)
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

        /// <summary>
        /// 是否定义了Flags特性
        /// </summary>
        /// <returns></returns>
        private static bool IsDefineFlagsAttribute<T>() where T : System.Enum
        {
            return typeof(T).IsDefined(typeof(FlagsAttribute), false);
        }

        #endregion
    }
}
