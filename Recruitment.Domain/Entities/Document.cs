using Recruitment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruitment.Domain.Entities
{
    public class Document
    {
        public long Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DocType DocumentType { get; set; }


        public List<Application> Applications { get; set; } = new();

    }
}
