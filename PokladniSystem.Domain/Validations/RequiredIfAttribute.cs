using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string _otherProperty { get; }
        private object _targetValue { get; }

        public RequiredIfAttribute(string otherProperty, object targetValue, string errorMessage): base(errorMessage)
        {
            _otherProperty = otherProperty;
            _targetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);

            if (otherPropertyInfo != null)
            {
                object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

                if (object.Equals(otherPropertyValue, _targetValue) && value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
