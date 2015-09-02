using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTest.ViewModels
{
    public class QueryOptions
    {
        [JsonProperty(PropertyName = "sortField")]
        public string SortField { get; set; }

        [JsonProperty(PropertyName = "sortOrder")]
        public SortOrder SortOrder { get; set; }

        [JsonProperty(PropertyName = "currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty(PropertyName = "totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        public QueryOptions()
        {
            SortField = "Id";
            SortOrder = SortOrder.DESC;
            CurrentPage = 1;
            PageSize = 4;
        }

        public string Sort
        {
            get
            {
                return string.Format("{0} {1}", SortField, SortOrder.ToString());
            }
        }
    }

    public enum SortOrder
    {
        DESC,
        ASC
    }
}