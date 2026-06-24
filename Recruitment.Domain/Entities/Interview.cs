using Recruitment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Entities
{
    public class Interview
    {
        public long Id { get; set; }
        public InterviewType Type { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public InterviewStatus Status { get; set; }
        public string Note { get; set; } = string.Empty;


        public long ApplicationId { get; set; }
        public Application? Application { get; set; }
    }
}
