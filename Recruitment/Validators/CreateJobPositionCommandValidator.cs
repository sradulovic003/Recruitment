using FluentValidation;
using Recruitment.Application.Features.JobPositions.Commands;

namespace Recruitment.API.Validators
{
    public class CreateJobPositionCommandValidator : AbstractValidator<CreateJobPositionCommand>
    {
        public CreateJobPositionCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Requirements).NotEmpty();
        }
    }
}
