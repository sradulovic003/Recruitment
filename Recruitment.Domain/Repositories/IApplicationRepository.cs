using Recruitment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Repositories
{
    public interface IApplicationRepository : IRepository<Application>
    {
        IEnumerable<Application> GetByUser(string userId);
        IEnumerable<Application> GetByJobAdvert(long jobAdvertId);
        Application? GetByIdWithDetails(long id);
        bool HasApplied(string userId, long jobAdvertId);
    }
}
