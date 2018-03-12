using System;
using System.Collections.Generic;
using System.Linq;
using AppRiver.JsonApi;
using AppRiver.JsonApi.Queries;

namespace Sandbox.Notes.Api.Notes.JsonApi.Extensions
{
    public static class QueryExtensions
    {
        public static IEnumerable<TSource> ApplyKeywordFilters<TSource>(this IEnumerable<TSource> source,
               Func<TSource, string> fieldSelector, IEnumerable<JsonApiFilter> filters)
        {
            foreach (var filter in filters)
            {
                var keywords = filter.Values.AsStrings().ToList();
                if (keywords.Count == 0) continue;
                source = source.WithAnyKeywordIn(fieldSelector, keywords);
            }
            return source;
        }

        public static IEnumerable<TSource> ApplyIsInFilters<TSource>(this IEnumerable<TSource> source,
            Func<TSource, string> fieldSelector, IEnumerable<JsonApiFilter> filters)
        {
            foreach (var filter in filters)
            {
                var values = filter.Values.AsStrings().ToList();
                if (values.Count == 0) continue;
                source = source.WithValueIn(fieldSelector, values);
            }
            return source;
        }

        public static IEnumerable<TSource> ApplyIsInRangeFilters<TSource, TField>(this IEnumerable<TSource> source,
            Func<TSource, TField> fieldSelector, IEnumerable<JsonApiFilter> filters) where TField : IComparable
        {
            foreach (var filter in filters.Where(f => f.IsRange))
            {
                var values = filter.Values.ToList();
                if (values.Count == 0) continue;
                var rangeStart = (TField)Convert.ChangeType(filter.RangeStart, typeof(TField));
                var rangeEnd = (TField)Convert.ChangeType(filter.RangeEnd, typeof(TField));
                source = source.WithValueInRange(fieldSelector, rangeStart, rangeEnd);
            }
            return source;
        }

        public static IEnumerable<TSource> WithValueIn<TSource>(this IEnumerable<TSource> source,
            Func<TSource, string> fieldSelector, ICollection<string> values)
        {
            foreach (var storyEntity in source)
            {
                var fieldValue = fieldSelector(storyEntity);
                if (string.IsNullOrWhiteSpace(fieldValue)) continue;
                if (fieldValue.IsIn(values)) yield return storyEntity;
            }
        }

        public static IEnumerable<TSource> WithValueInRange<TSource, TField>(this IEnumerable<TSource> source,
            Func<TSource, TField> fieldSelector, TField rangeStart, TField rangeEnd) where TField : IComparable
        {
            foreach (var storyEntity in source)
            {
                var fieldValue = fieldSelector(storyEntity);
                if (rangeStart.CompareTo(fieldValue) <= 0 || rangeEnd.CompareTo(fieldValue) > 0) yield return storyEntity;
            }
        }

        public static IEnumerable<TSource> WithAnyKeywordIn<TSource>(this IEnumerable<TSource> source,
            Func<TSource, string> fieldSelector, ICollection<string> keywords)
        {
            foreach (var storyEntity in source)
            {
                var fieldValue = fieldSelector(storyEntity);
                if (string.IsNullOrWhiteSpace(fieldValue)) continue;
                if (fieldValue.ContainsAnyKeyword(keywords)) yield return storyEntity;
            }
        }

        public static bool IsIn(this string fieldValue, ICollection<string> values)
        {
            if (values == null || values.Count == 0) return true;
            if (string.IsNullOrWhiteSpace(fieldValue)) return false;
            var lowerFieldValue = fieldValue.ToLower();
            return values
                .Select(value => value.Trim().ToLower())
                .Contains(lowerFieldValue);
        }

        public static bool ContainsAnyKeyword(this string fieldValue, ICollection<string> keywords)
        {
            if (keywords == null || keywords.Count == 0) return true;
            if (string.IsNullOrWhiteSpace(fieldValue)) return false;
            var lowerFieldValue = fieldValue.ToLower();
            return keywords
                .Select(keyword => keyword.Trim().ToLower())
                .Any(keyword => lowerFieldValue.Contains(keyword));
        }

        public static IEnumerable<TSource> SortedBy<TSource, TField>(this IEnumerable<TSource> source,
            Func<TSource, TField> fieldSelector, bool descending)
        {
            return descending
                ? source.OrderByDescending(fieldSelector)
                : source.OrderBy(fieldSelector);
        }
        
    }
}
