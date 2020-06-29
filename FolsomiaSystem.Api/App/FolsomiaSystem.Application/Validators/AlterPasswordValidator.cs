using FluentValidation;
using FolsomiaSystem.Application.DTOs;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.Validators
{
    class AlterPasswordValidator: AbstractValidator<AlterAdminUserInputs>
    {
        public AlterPasswordValidator()
        {

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("Required field")
                .NotEqual(p => p.ConfirmPassword).WithMessage("NewPassword must be equal ConfirmPassword");
            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("Required field")
                .NotEqual(p => p.NewPassword).WithMessage("ConfirmPassword must be equal NewPassword");

        }
    }
}
