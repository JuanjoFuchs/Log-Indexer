using System.Collections.Generic;

namespace LogIndexer.Core.Domain
{
    public abstract class ExtendedEntity : NamedEntity
    {
        public Dictionary<string, string> Extended { get; set; }
    }
}