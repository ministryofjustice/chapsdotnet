using ChapsDotNET.Frontend.Components.ListFilter;

namespace ChapsDotNET.Frontend.Components.Checkbox
{
    public class CheckboxModel : IFormFieldModel
    {
        public required string Id { get; set; }
        public string? Hint { get; set; }
        public bool Checked { get; set; }
        public required string Label { get; set; }
        public ValidFormFieldTypes Type { get; } = ValidFormFieldTypes.Checkbox;
    }
}
