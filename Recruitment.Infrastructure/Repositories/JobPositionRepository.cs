using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Infrastructure.Repositories
{
    public class JobPositionRepository : Repository<JobPosition>, IJobPositionRepository
    {
        public JobPositionRepository(RecruitmentContext context) : base(context) { }
    }
}
