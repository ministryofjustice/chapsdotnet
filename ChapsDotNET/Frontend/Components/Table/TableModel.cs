using System.Text.Json.Serialization;

namespace ChapsDotNET.Frontend.Components.Table
{
    //TODO: Implement other row scope options as needed
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RowScopes
    {
        col,
    }

    public enum SortOptions
    {
        none,
        ascending,
        descending
    }

    public enum TagColours
    {
        grey,
        green,
        turquoise,
        blue,
        purple,
        pink,
        red,
        orange,
        yellow
    }
    public class TableModel
    {
        public required List<HeaderCell> HeaderData { get; set; }
        public required List<Row> BodyData { get; set; }
        public string? Caption { get; set; }
        public bool CaptionIsPageTitle { get; set; }
        public string? ResultsTotal { get; set; }
        public string? ResultsFrom { get; set; }
        public string? ResultsTo { get; set; }
    }

    public interface Cell
    {
        public string Content { get; set; }
        public string? Url { get; set; }
    }

    public class HeaderCell : Cell
    {
        public required string Content { get; set; }
        public required RowScopes Scope { get; set; } = RowScopes.col;
        public SortOptions Sort { get; set; } = SortOptions.none;
        public string? Url { get; set; }
    }

    public class BodyCell : Cell
    {
        public required string Content { get; set; }
        public string? Url { get; set; }
        public TagColours? TagColour { get; set; }
    }
    public class Row
    {
        public required List<BodyCell> RowContent {get; set;}
    }
}
