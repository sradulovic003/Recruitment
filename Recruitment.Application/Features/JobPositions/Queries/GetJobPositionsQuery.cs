using MediatR;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobPositions.Queries
{
    public record GetJobPositionsQuery() : IRequest<List<JobPositionItem>>;

    public record JobPositionItem(long Id, string Title, JobType Type);

    public class GetJobPositionsQueryHandler : IRequestHandler<GetJobPositionsQuery, List<JobPositionItem>>
    {
        private readonly IUnitOfWork _uow;
        public GetJobPositionsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<JobPositionItem>> Handle(GetJobPositionsQuery request, CancellationToken ct)
        {
            var list = _uow.JobPositions.GetAll()
                .Select(p => new JobPositionItem(p.Id, p.Title, p.Type))
                .ToList();
            return Task.FromResult(list);
        }
    }
}
