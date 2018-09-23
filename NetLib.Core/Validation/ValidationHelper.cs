using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;
using ValidationContext = FluentValidation.ValidationContext;

namespace FrHello.NetLib.Core.Validation
{
    /// <summary>
    /// 数据校验辅助类
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// 获取对象的某一项属性错误
        /// </summary>
        /// <param name="obj">待校验的对象</param>
        /// <param name="propertyName">待校验的属性名</param>
        /// <param name="validator">FluentValidation验证器</param>
        /// <returns>错误信息</returns>
        public static ValidationResult GetPropertyErrorMsg(object obj, string propertyName, IValidator validator = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || obj == null)
            {
                return new ValidationResult(string.Empty);
            }

            if (validator != null)
            {
                var context = new ValidationContext(obj, null, new MemberNameValidatorSelector(new[] {propertyName}));
                var result = validator.Validate(context);

                var error = result.Errors.FirstOrDefault(x => x.PropertyName.Equals(propertyName))?.ErrorMessage;
                if (!string.IsNullOrEmpty(error))
                {
                    return new ValidationResult(error);
                }
            }

            var vc = new System.ComponentModel.DataAnnotations.ValidationContext(obj, null, null)
            {
                MemberName = propertyName
            };
            var res = new List<ValidationResult>();
            Validator.TryValidateProperty(obj.GetType().GetProperty(propertyName)?.GetValue(obj, null), vc, res);
            if (res.Any())
            {
                return new ValidationResult(string.Join(Environment.NewLine,
                    res.Select(r => r.ErrorMessage).ToArray()));
            }

            return new ValidationResult(string.Empty);
        }

        /// <summary>
        /// 获取对象的某一项属性错误
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">待校验的对象</param>
        /// <param name="propertyName">待校验的属性名</param>
        /// <param name="validator">FluentValidation验证器</param>
        /// <returns></returns>
        public static ValidationResult GetPropertyErrorMsg<T>(T obj, string propertyName, IValidator<T> validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            if (string.IsNullOrWhiteSpace(propertyName) || obj == null)
            {
                return new ValidationResult(string.Empty);
            }

            var context = new ValidationContext(obj, null, new MemberNameValidatorSelector(new[] {propertyName}));
            var result = validator.Validate(context);

            var error = result.Errors.FirstOrDefault(x => x.PropertyName.Equals(propertyName))?.ErrorMessage;
            if (!string.IsNullOrEmpty(error))
            {
                return new ValidationResult(error);
            }

            return new ValidationResult(string.Empty);
        }

        /// <summary>
        /// 获取对象的整体错误信息
        /// </summary>
        /// <param name="obj">待校验的对象</param>
        /// <param name="validator">FluentValidation验证器</param>
        /// <returns>错误信息</returns>
        public static ValidationResult GetErrorMsg(object obj, IValidator validator = null)
        {
            if (validator != null)
            {
                var result = validator.Validate(obj);

                if (result.Errors.Any())
                {
                    return new ValidationResult(result.Errors.First().ErrorMessage);
                }
            }

            if (obj != null)
            {
                var properties = obj.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var validationAttribute = propertyInfo.GetCustomAttribute(typeof(ValidationAttribute));
                    if (validationAttribute != null)
                    {
                        var vc = new System.ComponentModel.DataAnnotations.ValidationContext(obj, null, null)
                        {
                            MemberName = propertyInfo.Name
                        };
                        var res = new List<ValidationResult>();
                        Validator.TryValidateProperty(obj.GetType().GetProperty(propertyInfo.Name)?.GetValue(obj, null),
                            vc, res);
                        if (res.Count > 0)
                        {
                            return new ValidationResult(string.Join(Environment.NewLine,
                                res.Select(r => r.ErrorMessage).ToArray()));
                        }
                    }
                }
            }

            return new ValidationResult(string.Empty);
        }

        /// <summary>
        /// 获取对象的整体错误信息
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">待校验的对象</param>
        /// <param name="validator">FluentValidation验证器</param>
        /// <returns></returns>
        public static ValidationResult GetErrorMsg<T>(T obj, IValidator<T> validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            var result = validator.Validate(obj);

            if (result.Errors.Any())
            {
                return new ValidationResult(result.Errors.First().ErrorMessage);
            }

            return new ValidationResult(string.Empty);
        }

        /// <summary>
        /// 是否验证成功
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        public static bool IsValid(this ValidationResult validationResult)
        {
            return string.IsNullOrEmpty(validationResult?.ErrorMessage);
        }
    }
}
