using FluentValidation;
using FolsomiaSystem.Domain.Entities;
using System;

namespace FolsomiaSystem.Application.Validators
{
    public class FolsomiaSetupValidator : AbstractValidator<FolsomiaSetup>
    {
        public FolsomiaSetupValidator()
        {

            RuleFor(x => x.MaxTest)
            .NotNull()
            .WithMessage("MaxTest number is required")
            .GreaterThan(0)
            .WithMessage("MaxTest number must be greater than 0");

            RuleFor(p => p.MaxConcentration)
             .NotNull()
            .WithMessage("MaxConcentration number is required")
            .GreaterThan(0)
            .WithMessage("MaxConcentration number must be greater than 0");

        }

    }
}
