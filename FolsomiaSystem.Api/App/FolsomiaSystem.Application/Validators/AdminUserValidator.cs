using FluentValidation;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Infra.ES.CredentialSafe.Validators
{
    public class AdminUserValidator : AbstractValidator<AdminUser>
    {
        public AdminUserValidator()
        {

            RuleFor(p => p.UserName).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Required field");

        }
    }
}