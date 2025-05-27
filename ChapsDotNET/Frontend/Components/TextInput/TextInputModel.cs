using ChapsDotNET.Frontend.Components.ListFilter;

namespace ChapsDotNET.Frontend.Components.TextInput
{
    public class TextInputModel : IFormFieldModel
    {
        public required string Id { get; set; }
        public required string Label { get; set; }
        public string? Hint { get; set; }
        public ValidFormFieldTypes Type { get; } = ValidFormFieldTypes.TextInput;
        public string? Value { get; set; }
    }
}
