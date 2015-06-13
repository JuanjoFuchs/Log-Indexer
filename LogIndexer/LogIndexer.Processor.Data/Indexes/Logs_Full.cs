using System.Collections.Generic;
using System.Linq;
using LogIndexer.Core.Domain;
using Raven.Client.Indexes;

namespace LogIndexer.Processor.Data.Indexes
{
    public class Logs_Full : AbstractTransformerCreationTask<Log>
    {
        public Logs_Full()
        {
            TransformResults = logs => from log in logs
                let application = Include<Application>(log.ApplicationId)
                let environment = Include<Environment>(log.EnvironmentId)
                let dataSources = Include<DataSource>(log.DataSourceIds)
                select new
                {
                    log.Id,
                    log.Name,
                    log.ApplicationId,
                    log.EnvironmentId,
                    log.DataSourceIds,
                    Application = application,
                    Environment = environment,
                    DataSources = dataSources
                };
        }

        public class Result : Log
        {
            public Application Application { get; set; }
            public Environment Environment { get; set; }
            public List<DataSource> DataSources { get; set; }
        }
    }
}