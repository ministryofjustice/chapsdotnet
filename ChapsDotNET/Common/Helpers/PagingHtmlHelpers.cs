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
            StringBuilder pagingTags = new StringBuilder();
            //Prev Page
            if (pagedResult.CurrentPage > 1)
            {
                pagingTags.Append(GetTagString
                    ("Prev", pageUrl(pagedResult.CurrentPage - 1)));
            }
            //Page Numbers
            for (int i = 1; i <= pagedResult.PageCount; i++)
            {
                pagingTags.Append(GetTagString(i.ToString(), pageUrl(i)));
            }
            //Next Page
            if (pagedResult.CurrentPage < pagedResult.PageCount)
            {
                pagingTags.Append(GetTagString
                    ("Next", pageUrl(pagedResult.CurrentPage + 1)));
            }
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
    }
}
