using System.Collections.Generic;
using System.Linq;
using LogIndexer.Core.Domain;
using Raven.Client.Indexes;

namespace LogIndexer.Core.Data.Indexes
{
    public class Logs_Full : AbstractTransformerCreationTask<Log>
    {
        public Logs_Full()
        {
            TransformResults = logs => from log in logs
                let application = Include<Application>(log.ApplicationId)
                let environment = Include<Environment>(log.EnvironmentId)
                let dataSources = from dataSource in Include<DataSource>(log.DataSourceIds)
                                  let server = Include<Server>(dataSource.ServerId)
                                  select new
                                  {
                                      dataSource.Id,
                                      dataSource.Name,
                                      dataSource.ServerId,
                                      dataSource.Path,
                                      dataSource.File,
                                      Server = server
                                  }
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
            public List<DataSourceDTO> DataSources { get; set; }

            public class DataSourceDTO : DataSource
            {
                public Server Server { get; set; }
            }
        }
    }
}