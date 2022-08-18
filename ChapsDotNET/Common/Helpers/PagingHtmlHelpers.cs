using System.Text;
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

            //Add a leading space before the paging starts
            pagingTags.Append("<br/>");
            //Add how many total pages and what page we are on
            pagingTags.Append($"&nbsp;&nbsp;Page {pagedResult.CurrentPage} of {pagedResult.PageCount}&nbsp;&nbsp;");

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
                pagingTags.Append(GetTagString(i.ToString(), pageUrl(i)));
            }
            //Next Page
            if (pagedResult.CurrentPage < pagedResult.PageCount)
            {
                pagingTags.Append(GetTagString
                    ("Next", pageUrl(pagedResult.CurrentPage + 1)));
            }

            //Total Number of Records
            pagingTags.Append($"&nbsp;&nbsp;&nbsp; {pagedResult.RowCount} records");

            //paging tags
            return new HtmlString(pagingTags.ToString());
        }

        private static string GetTagString(string innerHtml, string? hrefValue)
        {
            var tag = new TagBuilder("a"); // Construct an <a> tag
            tag.MergeAttribute("class", "anchorstyle");
            tag.MergeAttribute("href", hrefValue);

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
