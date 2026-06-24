using MediatR;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Applications.Commands
{
    public record ChangeApplicationStatusCommand(long ApplicationId, ApplicationStatus NewStatus) : IRequest<bool>;

    public class ChangeApplicationStatusCommandHandler : IRequestHandler<ChangeApplicationStatusCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public ChangeApplicationStatusCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(ChangeApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var application = _uow.Applications.GetById(request.ApplicationId);
            if (application == null)
                return Task.FromResult(false);   

            application.Status = request.NewStatus;

            _uow.Applications.Update(application);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
