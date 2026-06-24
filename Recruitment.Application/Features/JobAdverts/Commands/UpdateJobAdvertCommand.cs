using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobAdverts.Commands
{
    public record UpdateJobAdvertCommand(
        long Id,
        long? JobPositionId,
        DateTime? StartDate,
        DateTime? Deadline,
        int? NumberOfOpenings) : IRequest<bool>;

    public class UpdateJobAdvertCommandHandler : IRequestHandler<UpdateJobAdvertCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public UpdateJobAdvertCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(UpdateJobAdvertCommand request, CancellationToken cancellationToken)
        {
            var advert = _uow.JobAdverts.GetById(request.Id);
            if (advert == null)
                return Task.FromResult(false); 

            if (request.JobPositionId.HasValue)
                advert.JobPositionId = request.JobPositionId.Value;

            if (request.StartDate.HasValue)
                advert.StartDate = request.StartDate.Value;

            if (request.Deadline.HasValue)
                advert.Deadline = request.Deadline.Value;

            if (request.NumberOfOpenings.HasValue)
                advert.NumberOfOpenings = request.NumberOfOpenings.Value;

            _uow.JobAdverts.Update(advert);
            _uow.SaveChanges();

            return Task.FromResult(true);   
        }
    }
}
