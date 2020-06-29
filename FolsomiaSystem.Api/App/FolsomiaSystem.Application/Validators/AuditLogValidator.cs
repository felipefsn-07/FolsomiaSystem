using FluentValidation;
using FolsomiaSystem.Domain.Entities;
using System;

namespace FolsomiaSystem.Application.Validators
{
    public class AuditLogValidator : AbstractValidator<AuditLog>
    {
        public AuditLogValidator()
        {

            RuleFor(p => p.MessageLog).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.OperationLog)
                .NotEmpty().WithMessage("Required field")
                .NotNull().WithMessage("OperationLog is invalid");
            RuleFor(p => p.StatusLog)
                .NotEmpty().WithMessage("Required field")
                .NotNull().WithMessage("StatusLog is invalid");
            RuleFor(p => p.DateLog).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.DateLog).Must(BeAValidDate).WithMessage("DateLog is invalid");

        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

    }
}
