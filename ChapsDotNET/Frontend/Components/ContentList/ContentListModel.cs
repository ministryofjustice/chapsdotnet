namespace ChapsDotNET.Frontend.Components.ContentList
{
    public class ContentListModel
    {
        public required string Id { get; set; }
        public required string Heading { get; set; }
        public bool ShowHeading { get; set; } // Set to true if the Content List heading should be the heading for the whole page
        public required List<ContentGroupModel> ContentGroups { get; set; }
    }

    public class ContentGroupModel
    {
        public required string Heading { get; set; }
        public string? Description { get; set; }
        public required List<ContentLinkModel> Links { get; set; }
    }
    public class ContentLinkModel
    {
        public required string Url { get; set; }
        public required string Label { get; set; }
        public string? Description { get; set; }
    }
}
