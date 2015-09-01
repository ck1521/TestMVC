using MVCTest.Models;
using MVCTest.ViewModels;
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

        public static MvcHtmlString BuildPreNextLinks(this HtmlHelper helper, QueryOptions qo, string actionName)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            return new MvcHtmlString(string.Format(
                "<nav>" +
                "   <ul class=\"pager\">" +
                "       <li class=\"previous {0}\">{1}</li>" +
                "       <li class=\"next {2}\">{3}</li>" +
                "   </ul>" +
                "</nav>",
                CheckLinkEnable(qo, true),
                BuildPreLink(urlHelper, qo, actionName),
                CheckLinkEnable(qo, false),
                BuildNextLink(urlHelper, qo, actionName)
                ));
        }

        #region Private Methods - Controller

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

        private static string CheckLinkEnable(QueryOptions qo, bool preOrNext)
        {
            bool disabled = false;
            if (preOrNext)
            {
                disabled = (qo.CurrentPage == 1);
            }
            else
            {
                disabled = (qo.CurrentPage == qo.TotalPages);
            }

            return disabled ? "disabled" : string.Empty;
        }

        private static string BuildPreLink(UrlHelper helper, QueryOptions qo, string actionName)
        {
            return string.Format(
                "<a href=\"{0}\"><span aria-hidden=\"true\">&larr;</span> Previous</a>",
                helper.Action(actionName, new
                {
                    SortOrder = qo.SortOrder,
                    SortField = qo.SortField,
                    CurrentPage = qo.CurrentPage - 1,
                    PageSize = qo.PageSize
                }));
        }

        private static string BuildNextLink(UrlHelper helper, QueryOptions qo, string actionName)
        {
            return string.Format(
                "<a href=\"{0}\">Next <span aria-hidden=\"true\">&rarr;</span></a>",
                helper.Action(actionName, new
                {
                    SortOrder = qo.SortOrder,
                    SortField = qo.SortField,
                    CurrentPage = qo.CurrentPage + 1,
                    PageSize = qo.PageSize
                }));
        }

        #endregion

        public static MvcHtmlString BuildKoSortLink(this HtmlHelper helper, string fieldName, string actionName, string sortField)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            return new MvcHtmlString(string.Format(
                "<a href=\"{0}\" data-bind=\"click: pagingService.sortEntitiesBy\"" +
                " data-sort-field=\"{1}\">{2} " +
                "<span data-bind=\"css: pagingService.buildSortIcon('{1}')\"></span></a>",
                urlHelper.Action(actionName),
                sortField,
                fieldName));
        }


        public static MvcHtmlString BuildSoPreNextLinks(this HtmlHelper helper, string actionName)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            return new MvcHtmlString(string.Format(
                "<nav>" +
                "   <ul class=\"pager\">" +
                "       <li data-bind=\"css: pagingService.buildPreviousClass()\">" +
                "           <a href=\"{0}\" data-bind=\"click: pagingService.previousPage\">Previous</a>" +
                "       </li>" +
                "       <li data-bind=\"css: pagingService.buildNextClass()\">" +
                "           <a href=\"{0}\" data-bind=\"click: pagingService.nextPage\">Next</a>" +
                "       </li>" +
                "   </ul>" +
                "</nav>",
                urlHelper.Action(actionName)));
        }
    }
}