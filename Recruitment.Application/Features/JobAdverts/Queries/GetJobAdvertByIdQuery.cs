using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobAdverts.Queries
{
    public record GetJobAdvertByIdQuery(long Id) : IRequest<JobAdvertDetails?>;

    public record JobAdvertDetails(
        long Id,
        string CompanyName,
        string PositionTitle,
        string PositionDescription,
        string Requirements,
        DateTime StartDate,
        DateTime Deadline,
        bool IsActive,
        int NumberOfOpenings);

    public class GetJobAdvertByIdQueryHandler : IRequestHandler<GetJobAdvertByIdQuery, JobAdvertDetails?>
    {
        private readonly IUnitOfWork _uow;

        public GetJobAdvertByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<JobAdvertDetails?> Handle(GetJobAdvertByIdQuery request, CancellationToken cancellationToken)
        {
            var a = _uow.JobAdverts.GetByIdWithDetails(request.Id);

            if (a == null)
                return Task.FromResult<JobAdvertDetails?>(null);

            var details = new JobAdvertDetails(
                a.Id,
                a.Company!.Name,
                a.JobPosition!.Title,
                a.JobPosition.Description,
                a.JobPosition.Requirements,
                a.StartDate,
                a.Deadline,
                a.IsActive,
                a.NumberOfOpenings);

            return Task.FromResult<JobAdvertDetails?>(details);
        }
    }
}
