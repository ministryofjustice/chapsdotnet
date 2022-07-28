using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ChapsDotNET
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var key = ButtonKeyFrom(controllerContext);
            var keyIsValid = IsValid(key);

            if (keyIsValid)
            {
                UpdateValueProviderIn(controllerContext, ValueFrom(key));
            }

            return keyIsValid;
        }

        private string ButtonKeyFrom(ControllerContext controllerContext)
        {
            var keys = controllerContext.HttpContext.Request.Params.AllKeys;
            return keys.FirstOrDefault(KeyStartsWithButtonName);
        }

        private static bool IsValid(string key)
        {
            return key != null;
        }

        private static string ValueFrom(string key)
        {
            var parts = key.Split(":".ToCharArray());
            return parts.Length < 2 ? null : parts[1];
        }

        private void UpdateValueProviderIn(ControllerContext controllerContext, string value)
        {
            if (string.IsNullOrEmpty(Argument)) return;

            //controllerContext.Controller.ValueProvider.GetValue(Argument).RawValue = new ValueProviderResult(value, value, null);
            var fakeForm = new FormCollection();
            //fakeForm = controllerContext.Controller.ValueProvider();
            ValueProviderResult bob = new ValueProviderResult(value, value, null);
            fakeForm.Add(Argument, value);
            controllerContext.Controller.ValueProvider = fakeForm.ToValueProvider();

        }

        private bool KeyStartsWithButtonName(string key)
        {
            return key.StartsWith(Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualToAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";

        public string OtherProperty { get; private set; }

        public NotEqualToAttribute(string otherProperty) : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                                                     .GetProperty(OtherProperty);

                var otherPropertyValue = otherProperty
                                            .GetValue(validationContext.ObjectInstance, null);

                if (value.Equals(otherPropertyValue))
                {
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "notequalto"
            };

            clientValidationRule.ValidationParameters.Add("otherproperty", OtherProperty);

            return new[] { clientValidationRule };
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PastDateAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Date must not be in the future";

        public override bool IsValid(object value)
        {
            if (value == null || (DateTime)value > DateTime.Now)
                return false;

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = DefaultErrorMessage,
                ValidationType = "pastdate"
            };
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FutureDateAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Date must not be in the past";

        public override bool IsValid(object value)
        {
            if (value == null || (DateTime)value < DateTime.Now)
                return false;

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = DefaultErrorMessage,
                ValidationType = "futuredate"
            };
        }
    }

}
