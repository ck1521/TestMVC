using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTest.Models
{
    public class QueryOptions
    {
        public string SortField { get; set; }
        public SortOrder SortOrder { get; set; }
        public QueryOptions()
        {
            SortField = "Id";
            SortOrder = SortOrder.DESC;
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