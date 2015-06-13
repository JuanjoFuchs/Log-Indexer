namespace LogIndexer.Core.Domain
{
    public abstract class NamedEntity : Entity
    {
        public string Name { get; set; }
    }
}