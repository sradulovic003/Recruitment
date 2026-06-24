using Recruitment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Entities
{
    public class Application
    {
        public long Id { get; set; }
        public DateTime AppliedAt { get; set; }
        public ApplicationStatus Status { get; set; }


        public long JobAdvertId { get; set; }
        public JobAdvert? JobAdvert { get; set; }

        public string UserId { get; set; } = string.Empty;

        public List<Document> Documents { get; set; } = new();

        public List<Interview> Interviews { get; set; } = new();

    }
}
