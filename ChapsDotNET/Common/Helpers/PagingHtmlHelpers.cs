﻿using System.Text;
using ChapsDotNET.Business.Models.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Common.Helpers
{
    public static class PagingHtmlHelpers
    {
        public static IHtmlContent PageLinks
            (this IHtmlHelper htmlHelper, IPagedResult pagedResult, Func<int, string?> pageUrl)
        {
            var pagingTags = new StringBuilder();

            pagingTags.Append($"<p>Page {pagedResult.CurrentPage} of {pagedResult.PageCount} of {pagedResult.RowCount} records</p>  ");

            if (pagedResult.CurrentPage > 1)
            {
                pagingTags.Append(GetImageString("pageButton-img-first-enabled", pageUrl(1)));
                pagingTags.Append(GetImageString("pageButton-img-previous-enabled", pageUrl(pagedResult.CurrentPage - 1)));
            }
            else
            {
                pagingTags.Append(GetImageString("pageButton-img-first-disabled", ""));
                pagingTags.Append(GetImageString("pageButton-img-previous-disabled", ""));
            }

            //Page Numbers
            for (var i = 1; i <= pagedResult.PageCount; i++)
            {
                if (pagedResult.CurrentPage == i)
                {
                    pagingTags.Append(GetTagString(i.ToString(), "", "pageButton pagenumber-disabled"));
                }
                else
                {
                    pagingTags.Append(GetTagString(i.ToString(), pageUrl(i), "pageButton"));
                }
                
            }
            //Next Page
            if (pagedResult.CurrentPage < pagedResult.PageCount)
            {
                pagingTags.Append(GetImageString("pageButton-img-next-enabled", pageUrl(pagedResult.CurrentPage + 1)));
                pagingTags.Append(GetImageString("pageButton-img-last-enabled", pageUrl(pagedResult.PageCount)));
            }
            else
            {
                pagingTags.Append(GetImageString("pageButton-img-next-disabled", ""));
                pagingTags.Append(GetImageString("pageButton-img-last-disabled", ""));
            }

            //paging tags
            return new HtmlString(pagingTags.ToString());
        }

        private static string GetTagString(string innerHtml, string? hrefValue, string? cssClass="")
        {
            var tag = new TagBuilder("a"); // Construct an <a> tag

            tag.MergeAttribute("class", !string.IsNullOrEmpty(cssClass) ? cssClass : "anchorstyle");

            if (!string.IsNullOrEmpty(hrefValue))
            {
                tag.MergeAttribute("href", hrefValue);
            }

            tag.InnerHtml.Append(" " + innerHtml + "  ");

            using var sw = new StringWriter();
            tag.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
            return sw.ToString();
        }

        private static string GetImageString(string innerHtml, string? hrefValue)
        {
            var tag = new TagBuilder("a"); // Construct an <a> tag
            tag.MergeAttribute("class", innerHtml);
            if (!string.IsNullOrEmpty(hrefValue))
            {
                tag.MergeAttribute("href", hrefValue);
            }

            using var sw = new StringWriter();
            tag.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
            return sw.ToString();
        }
    }
}
