using MediatR;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.JobPositions.Commands
{
    public record CreateJobPositionCommand(
        string Title,
        string Description,
        string Requirements,
        JobType Type) : IRequest<long>;

    public class CreateJobPositionCommandHandler : IRequestHandler<CreateJobPositionCommand, long>
    {
        private readonly IUnitOfWork _uow;

        public CreateJobPositionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<long> Handle(CreateJobPositionCommand request, CancellationToken cancellationToken)
        {
            var position = new JobPosition
            {
                Title = request.Title,
                Description = request.Description,
                Requirements = request.Requirements,
                Type = request.Type
            };

            _uow.JobPositions.Add(position);
            _uow.SaveChanges();

            return Task.FromResult(position.Id);
        }
    }
}
