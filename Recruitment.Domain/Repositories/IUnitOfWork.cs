using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository Companies { get; }
        IJobPositionRepository JobPositions { get; }
        IJobAdvertRepository JobAdverts { get; }
        IApplicationRepository Applications { get; }
        IInterviewRepository Interviews { get; }
        IDocumentRepository Documents { get; }

        int SaveChanges();
    }
}
