using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Core.Data
{
    public class Store
    {
        private static IDocumentStore _store;

        public static IDocumentStore Instance => _store ?? (_store = CreateStore());

        private static IDocumentStore CreateStore()
        {
            var store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "test" };
            store.Initialize();

            return store;
        }
    }
}