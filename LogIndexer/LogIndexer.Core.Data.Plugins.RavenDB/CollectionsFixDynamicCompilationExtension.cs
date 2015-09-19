using Raven.Database.Plugins;

namespace LogIndexer.Core.Data.Plugins.RavenDB
{
    public class CollectionsFixDynamicCompilationExtension : AbstractDynamicCompilationExtension
    {
        public override string[] GetNamespacesToImport()
        {
            return new[] {typeof (LogIndexerUtils).Namespace};
        }

        public override string[] GetAssembliesToReference()
        {
            return new[] {typeof (LogIndexerUtils).Assembly.Location};
        }
    }
}