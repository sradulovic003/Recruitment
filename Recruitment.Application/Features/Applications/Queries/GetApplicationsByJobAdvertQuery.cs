using MediatR;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Applications.Queries
{
    public record GetApplicationsByJobAdvertQuery(long JobAdvertId) : IRequest<List<ApplicationForAdvertItem>>;

    public record InterviewItem(long Id, InterviewType Type, DateTime ScheduledAt, string Location, InterviewStatus Status);

    // jedna prijava iz ugla admina (sa listom intervjua)
    public record ApplicationForAdvertItem(
        long Id,
        string CandidateId,
        string CandidateName,
        DateTime AppliedAt,
        ApplicationStatus Status,
        int DocumentsCount,
        List<InterviewItem> Interviews);

    public class GetApplicationsByJobAdvertQueryHandler : IRequestHandler<GetApplicationsByJobAdvertQuery, List<ApplicationForAdvertItem>>
    {
        private readonly IUnitOfWork _uow;

        public GetApplicationsByJobAdvertQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<ApplicationForAdvertItem>> Handle(GetApplicationsByJobAdvertQuery request, CancellationToken cancellationToken)
        {
            var items = _uow.Applications.GetByJobAdvert(request.JobAdvertId)
                .Select(app => new ApplicationForAdvertItem(
                    app.Id,
                    app.UserId,
                    "",  // u kontroleru
                    app.AppliedAt,
                    app.Status,
                    app.Documents.Count,
                    app.Interviews
                        .Select(i => new InterviewItem(i.Id, i.Type, i.ScheduledAt, i.Location, i.Status))
                        .ToList()))
                .ToList();

            return Task.FromResult(items);
        }
    }
}
