using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolsomiaSystem.Application.Validators
{
    class BaseValidator
    {
        public string MsgErrorValidator(ValidationResult results)
        {
            string msg = "{Validation:";
            foreach (var failure in results.Errors)
            {

                msg += ("{Attribute: " + failure.PropertyName + ", Mgs: " + failure.ErrorMessage + "},");
            }

            return msg + "}";
        }
    }
}
