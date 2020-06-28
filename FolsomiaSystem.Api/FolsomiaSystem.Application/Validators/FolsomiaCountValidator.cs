using FluentValidation;
using FluentValidation.Results;
using FolsomiaSystem.Domain.Entities;


namespace FolsomiaSystem.Application.Validators
{
    public class FolsomiaCountValidator : AbstractValidator<FolsomiaCount>
    {


        public FolsomiaCountValidator()
        {
            
            RuleFor(p => p.IdTest).NotEmpty().WithMessage("Required field");
            RuleFor(p => p.ImageFolsomiaOutlinedURL)
                .NotEmpty().WithMessage("Required field")
                .Matches(@"^.*\.(jpg|jpeg|png|JPG|JPEG|PNG)$").WithMessage("ImageFolsomiaOutlinedURL is invalid");

            RuleFor(p => p.ImageFolsomiaURL)
                  .NotEmpty().WithMessage("Required field")
                  .Matches(@"^.*\.(jpg|jpeg|png|JPG|JPEG|PNG)$").WithMessage("ImageFolsomiaURL is invalid");


            RuleFor(p => p.BackgroundImage)
                .NotNull().WithMessage("StatusLog is invalid");

        }

    }
}