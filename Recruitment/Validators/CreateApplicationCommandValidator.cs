using FluentValidation;
using Recruitment.Application.Features.Applications.Commands;

namespace Recruitment.API.Validators
{
    public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.JobAdvertId).GreaterThan(0);
            RuleFor(x => x.Documents).NotEmpty().WithMessage("Morate priloziti bar jedan dokument.");
        }
    }
}
