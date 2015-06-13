namespace LogIndexer.Core.Domain
{
    public class DataSource : NamedEntity
    {
        public string ServerId { get; set; }
        public string Path { get; set; }
        public string File { get; set; }
    }
}