using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobAdverts.Queries
{
    public record GetActiveJobAdvertsQuery() : IRequest<List<JobAdvertListItem>>;

    public record JobAdvertListItem(long Id, string CompanyName, string PositionTitle, DateTime Deadline, int NumberOfOpenings);

    public class GetActiveJobAdvertsQueryHandler : IRequestHandler<GetActiveJobAdvertsQuery, List<JobAdvertListItem>>
    {
        private readonly IUnitOfWork _uow;

        public GetActiveJobAdvertsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<JobAdvertListItem>> Handle(GetActiveJobAdvertsQuery request, CancellationToken cancellationToken)
        {
            var adverts = _uow.JobAdverts.GetActive()
                .Select(a => new JobAdvertListItem(
                    a.Id,
                    a.Company!.Name,
                    a.JobPosition!.Title,
                    a.Deadline,
                    a.NumberOfOpenings))
                .ToList();

            return Task.FromResult(adverts);
        }
    }
}
