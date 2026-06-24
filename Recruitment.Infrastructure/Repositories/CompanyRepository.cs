using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(RecruitmentContext context) : base(context) { }
    }
}
