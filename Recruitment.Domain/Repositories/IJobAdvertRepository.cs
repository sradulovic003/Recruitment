using Recruitment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Repositories
{
    public interface IJobAdvertRepository : IRepository<JobAdvert>
    {
        IEnumerable<JobAdvert> GetActive();
        JobAdvert? GetByIdWithDetails(long id);
    }
}
