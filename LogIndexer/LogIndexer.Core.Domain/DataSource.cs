namespace LogIndexer.Core.Domain
{
    public class DataSource : NamedEntity
    {
        public string Path { get; set; }
        public string File { get; set; }
    }
}