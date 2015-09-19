namespace LogIndexer.Core.Domain
{
    public class Record : Entity
    {
        public string Data { get; set; }
        public string DataSourceId { get; set; }
    }
}