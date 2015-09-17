using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogIndexer.Core.Domain
{
    public abstract class Filter : NamedEntity
    {
    }

    public class StronglyTypedFilter<T> : Filter
    {
        public int ModelId { get; set; }
        public SerializablePredicate<Func<Model<T>, bool>>  Predicate { get; set; }
    }

    public class Model<T> : NamedEntity
    {
        public T Definition { get; set; }
    }

    public class SerializablePredicate<T>
    {
        public string Serializable { get; set; }
        public T Predicate { get; set; }
    }

    public class UntypedFilter : Filter
    {
        public SerializablePredicate<Func<Record, bool>> Predicate { get; set; }
    }

    public class FilterTrigger : Entity
    {
        public int TriggerId { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }

    public class Trigger : NamedEntity
    {
        public int AlertId { get; set; }
    }

    public class Alert : NamedEntity
    {
         
    }

    public class AlertAction : NamedEntity
    {
        public string Type { get; set; }
        public string Destination { get; set; }
        public string Message { get; set; }
    }

    public class Index : NamedEntity
    {
         
    }

    public class Query : NamedEntity
    {
        public int IndexId { get; set; }
        public SerializablePredicate<Func<Index, IQueryable>> Predicate { get; set; }
    }

    public abstract class Transformation : NamedEntity
    {
        public int IndexId { get; set; }
    }

    public class StronglyTypedTransformation<T, K> : Transformation
    {
        public int ModelId { get; set; }
        public SerializablePredicate<Func<T, K>> Predicate { get; set; }
    }

    public class UntypedTransformation<K> : Transformation
    {
        public SerializablePredicate<Func<Record, K>> Predicate { get; set; }
    }

    public abstract class Grouping : NamedEntity
    {
        public int IndexId { get; set; } 
    }

    public class UntypedGrouping<K> : Grouping
    {
        public SerializablePredicate<Func<Func<Record, K>, Dictionary<K, Record[]>>> Predicate { get; set; }
    }

    public class StronglyTypedGrouping<T, K> : Grouping
    {
        public int ModelId { get; set; }
        public SerializablePredicate<Func<Func<T, K>, Dictionary<K, T[]>>> Predicate { get; set; }
    }

    public abstract class Reduction
    {
        public string Name { get; set; }
    }

    public class StronglyTypedReduction<T, K> : Reduction
    {
        public int ModelId { get; set; }
        public SerializablePredicate<Func<T, Func<T, K, T>>> Predicate { get; set; }
    }

    public class UntypedReduction<K> : Reduction
    {
        public SerializablePredicate<Func<K, Func<K, Record, K>>> Predicate { get; set; }
    }

    public class StatisticalDefinition : NamedEntity
    {
        public Reduction Reduction { get; set; }
    }

    public class User : NamedEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public Permission[] Premissions { get; set; }
    }

    public class Permission
    {
        public string Feature { get; set; }
        public string Access { get; set; } 
    }
}