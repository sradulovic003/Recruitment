using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobAdverts.Queries
{
    public record GetAllJobAdvertsQuery() : IRequest<List<JobAdvertListItem>>;

    public class GetAllJobAdvertsQueryHandler : IRequestHandler<GetAllJobAdvertsQuery, List<JobAdvertListItem>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllJobAdvertsQueryHandler(IUnitOfWork uow) { _uow = uow; }

        public Task<List<JobAdvertListItem>> Handle(GetAllJobAdvertsQuery request, CancellationToken ct)
        {
            var list = _uow.JobAdverts.GetAll()
                .Select(a => new JobAdvertListItem(
                    a.Id,
                    a.Company != null ? a.Company.Name : "",
                    a.JobPosition != null ? a.JobPosition.Title : "",
                    a.Deadline,
                    a.NumberOfOpenings))
                .ToList();
            return Task.FromResult(list);
        }
    }
}
