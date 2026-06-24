using MediatR;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Application.Features.Interviews.Commands
{
    public record DeleteInterviewCommand(long InterviewId) : IRequest<bool>;

    public class DeleteInterviewCommandHandler : IRequestHandler<DeleteInterviewCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public DeleteInterviewCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(DeleteInterviewCommand request, CancellationToken cancellationToken)
        {
            var interview = _uow.Interviews.GetById(request.InterviewId);
            if (interview == null)
                return Task.FromResult(false);  

            _uow.Interviews.Remove(interview);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
