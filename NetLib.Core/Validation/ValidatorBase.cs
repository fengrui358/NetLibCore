using FluentValidation;

namespace FrHello.NetLib.Core.Validation
{
    /// <summary>
    /// 泛型校验基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidatorBase<T> : AbstractValidator<T>
    {
    }
}
