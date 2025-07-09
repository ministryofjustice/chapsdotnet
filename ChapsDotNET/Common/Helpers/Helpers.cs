using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

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

    public class LayoutNameFilter : IActionFilter
    {
        /// <summary>
        /// Gets an array of strings representing Areas which have been updated to use the new frontend components.
        /// We can remove this and switch the _Layout.cshtml files once all Areas have been updated.
        /// </summary>
        public string[] UpdatedPageControllers { get; } = [
            "Users",
            "Salutations",
            "MPs",
            "PublicHolidays",
            "Admin",
            "Home"

        ];
        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _ = context.RouteData.Values.TryGetValue("controller", out object? controllerName);
            if (context.Result is ViewResult result && controllerName != null)
            {
                result.ViewData["LayoutName"] = Array.IndexOf(this.UpdatedPageControllers, controllerName.ToString()) > -1 ? "~/Frontend/_Layout.cshtml" : "_Layout";
            }

        }
        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Inherits from IActionFilter
        }
    }
}
