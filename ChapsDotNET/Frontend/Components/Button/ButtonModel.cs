using System.Text.Json.Serialization;

namespace ChapsDotNET.Frontend.Components.Button
{
    public class ButtonModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ValidButtonStyles
        {
            primary,
            secondary
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ValidElementTypes
        {
            anchor,
            input,
            button
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ValidButtonTypes
        {
            submit,
            reset,
            button
        }
        public ValidButtonStyles Type { get; set; } = ValidButtonStyles.primary;
        public required string Label { get; set; }
        public ValidElementTypes Element { get; set; } = ValidElementTypes.button;
        public string? Url { get; set; }
        public ValidButtonTypes InputType { get; set; } = ValidButtonTypes.button;
    }
}
