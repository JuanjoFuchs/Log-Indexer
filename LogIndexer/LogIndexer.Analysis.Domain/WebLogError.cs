using System;
using System.Collections.Generic;

namespace LogIndexer.Analysis.Domain
{
    public class WebLogError
    {
        public DateTime Date { get; set; }
        public string Controller { get; set; }
        public ICollection<LoggedException> Errors { get; set; }
    }
}