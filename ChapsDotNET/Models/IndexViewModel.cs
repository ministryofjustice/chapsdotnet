using ChapsDotNET.Frontend.Components.Alert;
using ChapsDotNET.Frontend.Components.Breadcrumbs;
using ChapsDotNET.Frontend.Components.Heading;
using ChapsDotNET.Frontend.Components.ListFilter;

public class IndexViewModel
{
    public AlertModel? Alert { get; set; }
    public required BreadcrumbsModel Breadcrumbs { get; set; }
    public required HeadingModel Heading { get; set; }
    public required ListFilterModel ListFilter { get; set; }
}

