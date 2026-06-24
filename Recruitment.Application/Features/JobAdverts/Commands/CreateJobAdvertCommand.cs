using MediatR;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobAdverts.Commands
{
    public record CreateJobAdvertCommand(long CompanyId, long JobPositionId, DateTime StartDate,DateTime Deadline,
        int NumberOfOpenings) : IRequest<long>;

    public class CreateJobAdvertCommandHandler : IRequestHandler<CreateJobAdvertCommand, long>
    {
        private readonly IUnitOfWork _uow;

        public CreateJobAdvertCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<long> Handle(CreateJobAdvertCommand request, CancellationToken cancellationToken)
        {
            var advert = new JobAdvert
            {
                CompanyId = request.CompanyId,
                JobPositionId = request.JobPositionId,
                StartDate = request.StartDate,
                Deadline = request.Deadline,
                NumberOfOpenings = request.NumberOfOpenings,
                IsActive = true   // novi oglas je odmah aktivan
            };

            _uow.JobAdverts.Add(advert);
            _uow.SaveChanges();

            return Task.FromResult(advert.Id);   // posle SaveChanges baza je dodelila Id
        }
    }
}
