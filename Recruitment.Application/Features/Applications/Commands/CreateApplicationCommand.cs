using MediatR;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Applications.Commands
{
    public record ApplicationDocumentInput(string FilePath, DocType DocumentType);

    public record CreateApplicationCommand(string UserId, long JobAdvertId, 
        List<ApplicationDocumentInput> Documents) : IRequest<CreateApplicationResult>;

    public record CreateApplicationResult(bool Success, string Message, long? ApplicationId);

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationResult>
    {
        private readonly IUnitOfWork _uow;

        public CreateApplicationCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<CreateApplicationResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var advert = _uow.JobAdverts.GetById(request.JobAdvertId);
            if (advert == null)
                return Task.FromResult(new CreateApplicationResult(false, "Oglas ne postoji.", null));

            if (!advert.IsActive)
                return Task.FromResult(new CreateApplicationResult(false, "Oglas nije aktivan.", null));

            if (_uow.Applications.HasApplied(request.UserId, request.JobAdvertId))
                return Task.FromResult(new CreateApplicationResult(false, "Vec ste aplicirali na ovaj oglas.", null));

            var application = new Recruitment.Domain.Entities.Application
            {
                UserId = request.UserId,
                JobAdvertId = request.JobAdvertId,
                AppliedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Pending,
                Documents = request.Documents
                    .Select(d => new Document
                    {
                        FilePath = d.FilePath,
                        DocumentType = d.DocumentType
                    })
                    .ToList()
            };

            _uow.Applications.Add(application);
            _uow.SaveChanges();

            return Task.FromResult(
                new CreateApplicationResult(true, "Uspesno ste aplicirali.", application.Id));
        }
    }

}
