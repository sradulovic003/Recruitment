using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Infrastructure.Repositories
{
    public class JobAdvertRepository : Repository<JobAdvert>, IJobAdvertRepository
    {
        public JobAdvertRepository(RecruitmentContext context) : base(context) { }

        // Svi aktivni oglasi (sa firmom i pozicijom)
        public IEnumerable<JobAdvert> GetActive() =>
            DbSet.Include(a => a.Company)
                 .Include(a => a.JobPosition)
                 .Where(a => a.IsActive)
                 .ToList();

        // Jedan oglas sa firmom i pozicijom
        public JobAdvert? GetByIdWithDetails(long id) =>
            DbSet.Include(a => a.Company)
                 .Include(a => a.JobPosition)
                 .FirstOrDefault(a => a.Id == id);

        public override IEnumerable<JobAdvert> GetAll() =>
            DbSet.Include(a => a.Company)
                 .Include(a => a.JobPosition)
                 .ToList();
    }
}
