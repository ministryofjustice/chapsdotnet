using ChapsDotNET.Frontend.Components.Pagination;
using ChapsDotNET.Frontend.Components.Table;
using System.Text.Json.Serialization;

namespace ChapsDotNET.Frontend.Components.ListFilter
{
    public class ListFilterModel
    {
        public string? Heading { get; set; }
        public List<IFormFieldModel>? Filters { get; set; }
        public List<SelectedFilter>? SelectedFilters { get; set; }
        public required TableModel Table { get; set; }
        public PaginationModel? Pagination { get; set; }
        public bool PanelIsOpen { get; set; }
        public string? ResetUrl { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValidFormFieldTypes
    {
        TextInput,
        CheckboxGroup,
        Checkbox,
    }
    public interface IFormFieldModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string? Hint { get; set; }
        public ValidFormFieldTypes Type { get; }
    }

    public class SelectedFilter
    {
        public required string Id { get; set; }
        public string? Heading { get; set; }
        public string? Value {get; set;}
        public List<SelectedFilter>? Options { get; set; }
    }
}
