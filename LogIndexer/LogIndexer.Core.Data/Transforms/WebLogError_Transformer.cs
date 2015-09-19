using System;
using System.Linq;
using System.Text.RegularExpressions;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Data.Plugins;
using LogIndexer.Core.Domain;
using Raven.Client.Indexes;

namespace LogIndexer.Core.Data.Transforms
{
    public class WebLogError_Transformer : AbstractTransformerCreationTask<Record>
    {
        public WebLogError_Transformer()
        {
            TransformResults = records => records
                .Select(record => TransformWith<Record, WebLog>("WebLog/Transformer", record).FirstOrDefault())
                .Where(webLog => webLog != null && webLog.Level == "Error")
                .Select(webLog => new { webLog, errors = webLog.Message.Split(new[] { "--->" }, StringSplitOptions.RemoveEmptyEntries) })
                .Where(record => record.errors.Length >= 1)
                .Select(record => new
                {
                    record.webLog.Date,
                    record.webLog.Controller,
                    Errors = LogIndexerUtils.Select(record.errors, error => new
                    {
                        Type = Regex.Match(error, @"(?<type>[a-zA-Z\.]+)\:").Groups["type"].Value,
                        Message = error
                    })
                });
        }
    }
}