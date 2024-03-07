using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpedizioniEpicode.Validations
{
   

    public class XORValidation : ValidationAttribute
    {
        private string[] _otherPropertyNames;

        public XORValidation(params string[] otherPropertyNames)
        {
            _otherPropertyNames = otherPropertyNames;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var otherPropertyName in _otherPropertyNames)
            {
                var otherPropery = validationContext.ObjectType.GetProperty(otherPropertyName);
                var otherPropertyValue = otherPropery.GetValue(validationContext.ObjectInstance);

                if (value != null && otherPropertyValue != null)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}