using Recruitment.Domain.Repositories;
using Recruitment.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecruitmentContext _context;

        private ICompanyRepository? _companies;
        private IJobPositionRepository? _jobPositions;
        private IJobAdvertRepository? _jobAdverts;
        private IApplicationRepository? _applications;
        private IInterviewRepository? _interviews;
        private IDocumentRepository? _documents;

        public UnitOfWork(RecruitmentContext context)
        {
            _context = context;
        }

        public ICompanyRepository Companies =>
            _companies ??= new CompanyRepository(_context);

        public IJobPositionRepository JobPositions =>
            _jobPositions ??= new JobPositionRepository(_context);

        public IJobAdvertRepository JobAdverts =>
            _jobAdverts ??= new JobAdvertRepository(_context);

        public IApplicationRepository Applications =>
            _applications ??= new ApplicationRepository(_context);

        public IInterviewRepository Interviews =>
            _interviews ??= new InterviewRepository(_context);

        public IDocumentRepository Documents =>
            _documents ??= new DocumentRepository(_context);

        public int SaveChanges() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}
