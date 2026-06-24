using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Infrastructure.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        public ApplicationRepository(RecruitmentContext context) : base(context) { }

        // Prijave jednog kandidata (sa oglasom)
        public IEnumerable<Application> GetByUser(string userId) =>
            DbSet.Include(app => app.JobAdvert)!
                    .ThenInclude(a => a!.Company)
                 .Include(app => app.JobAdvert)!
                    .ThenInclude(a => a!.JobPosition)
                 .Where(app => app.UserId == userId)
                 .ToList();

        // Sve prijave za jedan oglas (sa dokumentima i intervjuima)
        public IEnumerable<Application> GetByJobAdvert(long jobAdvertId) =>
            DbSet.Include(app => app.Documents)
                 .Include(app => app.Interviews)
                 .Where(app => app.JobAdvertId == jobAdvertId)
                 .ToList();

        // Jedna prijava sa svim detaljima
        public Application? GetByIdWithDetails(long id) =>
            DbSet.Include(app => app.JobAdvert)
                 .Include(app => app.Documents)
                 .Include(app => app.Interviews)
                 .FirstOrDefault(app => app.Id == id);

        // Da li je korisnik vec aplicirao na oglas
        public bool HasApplied(string userId, long jobAdvertId) =>
            DbSet.Any(app => app.UserId == userId && app.JobAdvertId == jobAdvertId);
    }
}
