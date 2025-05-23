using ChapsDotNET.Frontend.Components.Checkbox;
using ChapsDotNET.Frontend.Components.ListFilter;

namespace ChapsDotNET.Frontend.Components.CheckboxGroup
{
    public class CheckboxGroupModel : IFormFieldModel
    {
        public required string Id { get; set; }
        public required string Label { get; set; }
        public string? Hint { get; set; }
        public ValidFormFieldTypes Type { get; } = ValidFormFieldTypes.CheckboxGroup;
        public required List<CheckboxModel> Options { get; set; }
    }
}
