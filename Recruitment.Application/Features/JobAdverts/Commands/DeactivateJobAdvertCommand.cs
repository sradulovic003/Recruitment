using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobAdverts.Commands
{
    public record DeactivateJobAdvertCommand(long Id) : IRequest<bool>;

    public class DeactivateJobAdvertCommandHandler : IRequestHandler<DeactivateJobAdvertCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public DeactivateJobAdvertCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(DeactivateJobAdvertCommand request, CancellationToken cancellationToken)
        {
            var advert = _uow.JobAdverts.GetById(request.Id);
            if (advert == null)
                return Task.FromResult(false);

            advert.IsActive = false;   // oglas ostaje u bazi, samo se gasi

            _uow.JobAdverts.Update(advert);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
