using Recruitment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Entities
{
    public class JobPosition
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType Type { get; set; }
        public string Requirements { get; set; } = string.Empty;


        public List<JobAdvert> JobAdverts { get; set; } = new();
    }
}
