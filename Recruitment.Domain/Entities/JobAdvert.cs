using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Entities
{
    public class JobAdvert
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfOpenings { get; set; }


        public long CompanyId { get; set; }
        public Company? Company { get; set; }

        public long JobPositionId { get; set; }
        public JobPosition? JobPosition { get; set; }

        public List<Application> Applications { get; set; } = new();
    }
}
