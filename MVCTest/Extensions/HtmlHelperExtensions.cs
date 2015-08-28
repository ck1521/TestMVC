using MVCTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString HtmlConvertToJson(this HtmlHelper helper, object model)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            return new HtmlString(JsonConvert.SerializeObject(model, settings));
        }

        public static MvcHtmlString BuildSortLink(this HtmlHelper helper, string field, string action, string sortField, QueryOptions qo)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            bool sameField = qo.SortField == sortField;

            return new MvcHtmlString(string.Format("<a href=\"{0}\">{1} {2}</a>",
                urlHelper.Action(action,
                new
                {
                    SortField = sortField,
                    SortOrder = (sameField && (qo.SortOrder == SortOrder.DESC) ? SortOrder.ASC : SortOrder.DESC)
                }),
                    field,
                    BuildSortIcon(sameField, qo)
                    ));
        }


        private static string BuildSortIcon(bool sameField, QueryOptions qo)
        {
            StringBuilder iconStr = new StringBuilder("sort");
            if (sameField)
            {
                iconStr.Append("-by-alphabet");
                if (qo.SortOrder == SortOrder.DESC)
                {
                    iconStr.Append("-alt");
                }
            }
            return string.Format("<span class=\"{0} {1}{2}\"></span>", "glyphicon", "glyphicon-", iconStr);
        }


    }
}