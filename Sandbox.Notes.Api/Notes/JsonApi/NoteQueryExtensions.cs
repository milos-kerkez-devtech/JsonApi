using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.JsonApi
{
    public static class NoteQueryExtensions
    {
        public static IEnumerable<Note> Filter(this IEnumerable<Note> notes, JsonApiQuery query, NoteMappingService noteMappingService)
        {
            return notes
                .ApplyKeywordFilters(s => s.Text, query.FindFilters("text"))
                .ApplyKeywordFilters(s => s.Creator, query.FindFilters("creator"))
                .ApplyIsInRangeFilters(s => s.CreatedOn, query.FindFilters("createdOn"));
        }

        public static IEnumerable<Note> Sort(this IEnumerable<Note> notes, JsonApiQuery query, NoteMappingService noteMappingService)
        {
            foreach (var sort in query.Sorts.Reverse())
            {
                if ("text".Equals(sort.Field, StringComparison.OrdinalIgnoreCase))
                {
                    notes = notes.SortedBy(s => s.Text, sort.Descending);
                }

                if ("createdOn".Equals(sort.Field, StringComparison.OrdinalIgnoreCase))
                {
                    notes = notes.SortedBy(s => s.CreatedOn, sort.Descending);
                }

                if ("id".Equals(sort.Field, StringComparison.OrdinalIgnoreCase))
                {
                    notes = notes.SortedBy(s => s.Id, sort.Descending);
                }


                if ("creator".Equals(sort.Field, StringComparison.OrdinalIgnoreCase))
                {
                    notes = notes.SortedBy(s => s.Creator, sort.Descending);
                }
            }
            return notes;
        }

        public static IEnumerable<Note> Paginate(this IEnumerable<Note> notes, JsonApiQuery query)
        {
            if (query.Page.Count > 0)
            {
                notes = notes.Skip(query.Page.First).Take(query.Page.Count);
            }
            return notes;
        }
    }
}
