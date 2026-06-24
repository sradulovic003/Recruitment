using FluentValidation;
using Recruitment.Application.Features.JobAdverts.Commands;

namespace Recruitment.API.Validators
{
    public class UpdateJobAdvertCommandValidator : AbstractValidator<UpdateJobAdvertCommand>
    {
        public UpdateJobAdvertCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            // proveravamo samo ako je polje poslato 
            When(x => x.JobPositionId.HasValue, () =>
                RuleFor(x => x.JobPositionId!.Value).GreaterThan(0));

            When(x => x.NumberOfOpenings.HasValue, () =>
                RuleFor(x => x.NumberOfOpenings!.Value).GreaterThan(0));

            When(x => x.StartDate.HasValue && x.Deadline.HasValue, () =>
                RuleFor(x => x.Deadline!.Value)
                    .GreaterThan(x => x.StartDate!.Value)
                    .WithMessage("Rok mora biti posle datuma pocetka."));
        }
    }
}

