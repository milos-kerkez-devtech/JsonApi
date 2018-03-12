using System;
using System.Linq;
using AppRiver.JsonApi;

namespace Sandbox.Notes.Api.Notes.JsonApi.Utility
{
    public static class JsonApiLinkUtility
    {
        public static string BuildNextLink(JsonApiQuery query, int totalNumberOfEntities, string baseQueryLink, string requestDisplayUrl)
        {
            var nextLink = "";
            if (query.Page.Count + query.Page.First >= totalNumberOfEntities) return nextLink;
            var pageFirstElement = query.Page.Count + query.Page.First;

            nextLink = AddPageToLink(query, baseQueryLink, requestDisplayUrl, pageFirstElement);

            nextLink = AddFiltersToLink(query, nextLink);

            nextLink = AddSortsToLink(query, nextLink);

            nextLink = AddIncludesToLink(query, nextLink);
            return nextLink;
        }



        public static string BuildLastLink(JsonApiQuery query, int totalNumberOfEntities, string baseQueryLink, string requestDisplayUrl)
        {
            var lastLink = "";
            if (query.Page.Count + query.Page.First >= totalNumberOfEntities) return requestDisplayUrl;
            var remainderForLastPage = Math.IEEERemainder(totalNumberOfEntities, query.Page.Count);
            var pageFirstElement = (int)remainderForLastPage == 0 ? totalNumberOfEntities - query.Page.Count : totalNumberOfEntities - remainderForLastPage;


            lastLink = AddPageToLink(query, baseQueryLink, requestDisplayUrl, pageFirstElement);

            lastLink = AddFiltersToLink(query, lastLink);

            lastLink = AddSortsToLink(query, lastLink);

            lastLink = AddIncludesToLink(query, lastLink);

            return lastLink;
        }

        private static string AddPageToLink(JsonApiQuery query, string baseQueryLink, string requestDisplayUrl, double pageFirstElement)
        {
            return new Uri(requestDisplayUrl).GetLeftPart(UriPartial.Authority) + baseQueryLink +
                       "page[count]=" + query.Page.Count + "&page[first]=" +
                       pageFirstElement;
        }

        private static string AddFiltersToLink(JsonApiQuery query, string link)
        {
            if (!query.Filters.Any()) return link;

            return query.Filters.Aggregate(link, (current1, filter) => 
            filter.Values.Aggregate(current1, (current, filterValue) => 
            current + "&filter[" + filter.Field + "]=" + filterValue));
        }

        private static string AddSortsToLink(JsonApiQuery query, string link)
        {
            if (!query.Sorts.Any()) return link;

            link += "&sort=";
            foreach (var sort in query.Sorts)
            {
                link += sort;
                if (!query.Sorts.Last().Equals(sort))
                {
                    link += ",";
                }
            }
            return link;
        }

        private static string AddIncludesToLink(JsonApiQuery query, string link)
        {
            if (!query.Includes.Any()) return link;

            link += "&include=";
            foreach (var include in query.Includes)
            {
                link += include;
                if (!query.Includes.Last().Equals(include))
                {
                    link += ",";
                }
            }
            return link;
        }

    }
}
