using System.Diagnostics.CodeAnalysis;

namespace ChapsDotNET.Frontend.Components.Breadcrumbs
{
    public class BreadcrumbModel
    {
        public required string Label { get; set; }
        public required string Url { get; set; }
    }
    public class BreadcrumbsModel
    {
        public required List<BreadcrumbModel> Links { get; set; }
        [SetsRequiredMembers]
        public BreadcrumbsModel(List<BreadcrumbModel> links) => Links = links;
    }
}
