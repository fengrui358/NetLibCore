using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FrHello.NetLib.Core.Regex;
using FrHello.NetLib.Core.Validation;
using Xunit;

namespace NetLib.Core.Test.Validation.Test
{
    /// <summary>
    /// ValidationTest
    /// </summary>
    public class ValidationTest
    {
        /// <summary>
        /// FluentValidationTest
        /// </summary>
        [Fact]
        public void FluentValidationTest()
        {
            var mockValidationObject = new MockValidationObject("dsa@");
            var validatorBase = new ValidatorBase<MockValidationObject>();
            validatorBase.RuleFor(s => s.Email).Must(RegexHelper.CheckEmail).WithMessage("Email格式不正确");
            var validateResult = ValidationHelper.GetErrorMsg(mockValidationObject, validatorBase);

            Assert.False(validateResult.IsValid());
            Assert.Equal("Email格式不正确", validateResult.ErrorMessage);

            var validateResultWithProperty = ValidationHelper.GetPropertyErrorMsg(mockValidationObject,
                nameof(MockValidationObject.Email), validatorBase);
            Assert.False(validateResultWithProperty.IsValid());
            Assert.Equal("Email格式不正确", validateResultWithProperty.ErrorMessage);

            var validateResultWithAnnotation = ValidationHelper.GetPropertyErrorMsg(mockValidationObject,
                nameof(MockValidationObject.Email));
            Assert.False(validateResultWithAnnotation.IsValid());
            Assert.Equal("Email特性格式不正确", validateResultWithAnnotation.ErrorMessage);
        }

        private class MockValidationObject
        {
            [EmailAddress(ErrorMessage = "Email特性格式不正确")]
            public string Email { get; }

            public MockValidationObject(string email)
            {
                Email = email;
            }
        }
    }
}
