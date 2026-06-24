using MediatR;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Interviews.Commands
{
    public record ScheduleInterviewCommand(long ApplicationId, InterviewType Type, string Location, 
        DateTime ScheduledAt, string Note) : IRequest<ScheduleInterviewResult>;

    public record ScheduleInterviewResult(bool Success, string Message, long? InterviewId);

    public class ScheduleInterviewCommandHandler : IRequestHandler<ScheduleInterviewCommand, ScheduleInterviewResult>
    {
        private readonly IUnitOfWork _uow;

        public ScheduleInterviewCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<ScheduleInterviewResult> Handle(
            ScheduleInterviewCommand request, CancellationToken cancellationToken)
        {
            // prijava mora da postoji
            var application = _uow.Applications.GetById(request.ApplicationId);
            if (application == null)
                return Task.FromResult(new ScheduleInterviewResult(false, "Prijava ne postoji.", null));

            var interview = new Recruitment.Domain.Entities.Interview
            {
                ApplicationId = request.ApplicationId,
                Type = request.Type,
                Location = request.Location,
                ScheduledAt = request.ScheduledAt,
                Status = InterviewStatus.Scheduled,   // novozakazan intervju
                Note = request.Note
            };

            _uow.Interviews.Add(interview);
            _uow.SaveChanges();

            return Task.FromResult(
                new ScheduleInterviewResult(true, "Intervju zakazan.", interview.Id));
        }
    }
}
