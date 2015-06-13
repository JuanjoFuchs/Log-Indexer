using System;

namespace LogIndexer.Analysis.Domain
{
    public class WebLog
    {
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Controller { get; set; }
        public string Message { get; set; }
    }
}