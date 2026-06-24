using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Entities
{
    public class Company
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string WebSite { get; set; } = string.Empty;


        public List<JobAdvert> JobAdverts { get; set; } = new();
    }
}
