using FluentValidation;
using Recruitment.Application.Features.Applications.Commands;

namespace Recruitment.API.Validators
{
    public class ChangeApplicationStatusCommandValidator : AbstractValidator<ChangeApplicationStatusCommand>
    {
        public ChangeApplicationStatusCommandValidator()
        {
            RuleFor(x => x.ApplicationId).GreaterThan(0);
            RuleFor(x => x.NewStatus).IsInEnum();
        }
    }
}
