using System.Linq;
using LogIndexer.Analysis.Domain;

namespace LogIndexer.RoslynCompiler.Template
{
    public class LinqQuery
    {
         public static object Compile(IQueryable<WebLog> query)
         {
            return from webLog in query
                   select webLog.Level;
         }
    }
}