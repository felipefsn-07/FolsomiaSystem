using FluentValidation;
using FolsomiaSystem.Application.DTOs;

namespace FolsomiaSystem.Application.Validators
{
    public class AlterPasswordValidator : AbstractValidator<AlterAdminUserInputs>
    {
        public AlterPasswordValidator()
        {

            RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.ConfirmPassword).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.NewPassword).Equal(p => p.ConfirmPassword).WithMessage("NewPassword must be equal ConfirmPassword");
            
        }
    }
}
