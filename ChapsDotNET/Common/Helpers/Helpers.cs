using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Common.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent Timeago(this IHtmlHelper helper, DateTime? dateTime)
        {
            var contentBuilder = new HtmlContentBuilder();

            if (dateTime == null)
            {
                var tag = new TagBuilder("span");
                tag.InnerHtml.Append("Never");
                contentBuilder.AppendHtml(tag);
            }
            else
            {
                DateTime output = dateTime.Value;
                var tag = new TagBuilder("abbr");
                tag.AddCssClass("timeago");
                tag.Attributes.Add("title", output.ToString("s"));
                tag.InnerHtml.Append(output.ToString("f"));
                contentBuilder.AppendHtml(tag);
            }

            return contentBuilder;
        }
    }

    public static class UrlHelpers
    {
        public static string GetTpUrl(HttpContext context)
        {
            var currentEnvironment = context.Request.Host.Value.ToString();
            string? environment;
            if (currentEnvironment.Contains("staging"))
            {
                environment = "staging.";
            }
            else if (currentEnvironment.Contains("dev") || currentEnvironment.Contains("localhost"))
            {
                environment = "dev.";
            }
            else
            {
                environment = "production.";
            }

            return $"https://chaps.{environment}net.tp.dsd.io/Chaps_deploy";
        }
    }
}


