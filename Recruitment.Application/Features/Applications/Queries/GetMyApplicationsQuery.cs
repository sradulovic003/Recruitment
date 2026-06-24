using MediatR;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Applications.Queries
{
    public record GetMyApplicationsQuery(string UserId) : IRequest<List<MyApplicationItem>>;

    public record MyApplicationItem(long Id, string CompanyName, string PositionTitle, DateTime AppliedAt, ApplicationStatus Status);

    public class GetMyApplicationsQueryHandler : IRequestHandler<GetMyApplicationsQuery, List<MyApplicationItem>>
    {
        private readonly IUnitOfWork _uow;

        public GetMyApplicationsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<MyApplicationItem>> Handle(GetMyApplicationsQuery request, CancellationToken cancellationToken)
        {
            var items = _uow.Applications.GetByUser(request.UserId)
                .Select(app => new MyApplicationItem(
                    app.Id,
                    app.JobAdvert!.Company!.Name,
                    app.JobAdvert.JobPosition!.Title,
                    app.AppliedAt,
                    app.Status))
                .ToList();

            return Task.FromResult(items);
        }
    }
}
