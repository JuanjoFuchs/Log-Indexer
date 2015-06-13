using System.Collections.Generic;

namespace LogIndexer.Core.Domain
{
    public class Log : NamedEntity
    {
        public string ApplicationId { get; set; }
        public string EnvironmentId { get; set; }

        public ICollection<string> DataSourceIds { get; set; }

        public Log()
        {
            DataSourceIds = new List<string>();
        }
    }
}