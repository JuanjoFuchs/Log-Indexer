using System;
using System.Collections.Generic;
using System.Globalization;

namespace LogIndexer.Core.Data.Plugins
{
    public static class LogIndexerUtils
    {
        public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (var x in source)
                yield return selector(x);
        }

        public static DateTimeOffset? TryParseExact(string input, string format)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            const DateTimeStyles none = DateTimeStyles.None;

            DateTimeOffset dateTimeOffset;
            if (!DateTimeOffset.TryParseExact(input, format, currentCulture, none, out dateTimeOffset))
                return null;

            return dateTimeOffset;
        }
    }
}
