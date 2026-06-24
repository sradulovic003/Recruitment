using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Companies.Queries
{
    public record GetCompaniesQuery() : IRequest<List<CompanyItem>>;

    public record CompanyItem(long Id, string Name, string City);

    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<CompanyItem>>
    {
        private readonly IUnitOfWork _uow;
        public GetCompaniesQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<CompanyItem>> Handle(GetCompaniesQuery request, CancellationToken ct)
        {
            var list = _uow.Companies.GetAll()
                .Select(c => new CompanyItem(c.Id, c.Name, c.City))
                .ToList();
            return Task.FromResult(list);
        }
    }
}
