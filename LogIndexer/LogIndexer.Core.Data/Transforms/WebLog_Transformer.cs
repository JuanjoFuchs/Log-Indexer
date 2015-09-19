using System;
using System.Globalization;
using System.Linq;
using LogIndexer.Core.Domain;
using Raven.Client.Indexes;

namespace LogIndexer.Core.Data.Transforms
{
    public class WebLog_Transformer : AbstractTransformerCreationTask<Record>
    {
        public WebLog_Transformer()
        {
            TransformResults = records => records
                .Select(record => record.Data.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries))
                .Where(data => data.Length >= 4 && data[0].Length == 14)
                .Select(data => new
                {
                    Date = DateTimeOffset.ParseExact(data[0], "yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                    Level = data[1],
                    Controller = data[2],
                    Message = string.Join(System.Environment.NewLine, data.Except(new[] { data[0], data[1], data[2] }))
                });
        }
    }
}