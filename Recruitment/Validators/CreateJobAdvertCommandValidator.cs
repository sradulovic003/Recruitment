using FluentValidation;
using Recruitment.Application.Features.JobAdverts.Commands;

namespace Recruitment.API.Validators
{
    public class CreateJobAdvertCommandValidator : AbstractValidator<CreateJobAdvertCommand>
    {
        public CreateJobAdvertCommandValidator()
        {
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.JobPositionId).GreaterThan(0);
            RuleFor(x => x.NumberOfOpenings).GreaterThan(0);
            RuleFor(x => x.Deadline)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Rok mora biti posle datuma pocetka.");
        }
    }
}
