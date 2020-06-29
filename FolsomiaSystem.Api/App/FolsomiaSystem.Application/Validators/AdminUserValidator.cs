using FluentValidation;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Domain.Entities;

namespace FolsomiaSystem.Application.Validators
{
    public class AdminUserValidator : AbstractValidator<AdminUserInputs>
    {
        public AdminUserValidator()
        {

            RuleFor(p => p.UserName).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Required field");

        }
    }
}