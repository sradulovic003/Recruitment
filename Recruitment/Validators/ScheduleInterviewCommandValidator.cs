using FluentValidation;
using Recruitment.Application.Features.Interviews.Commands;

namespace Recruitment.API.Validators
{
    public class ScheduleInterviewCommandValidator : AbstractValidator<ScheduleInterviewCommand>
    {
        public ScheduleInterviewCommandValidator()
        {
            RuleFor(x => x.ApplicationId).GreaterThan(0);
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ScheduledAt).GreaterThan(DateTime.UtcNow)
                .WithMessage("Termin intervjua mora biti u buducnosti.");
        }
    }
}
