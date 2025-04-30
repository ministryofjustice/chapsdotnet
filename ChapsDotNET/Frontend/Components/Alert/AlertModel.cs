using System;
using System.Text.Json.Serialization;

namespace ChapsDotNET.Frontend.Components.Alert
{
    public class AlertModel
    {
        public static AlertTypes GetAlertTypeFromStatus(string status)
        {
            return status switch
            {
                "success" => AlertTypes.success,
                "error" => AlertTypes.error,
                "warning" => AlertTypes.warning,
                _ => AlertTypes.information,
            };
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum AlertTypes
        {
            information,
            success,
            warning,
            error
        }
        public string? Heading { get; set; }
        // A shorthand version of Content, automatically prefixed with the alert type e.g. "Error: User not updated"
        // Used in aria-label
        public required string Summary { get; set; }
        public required string Content { get; set; }
        public required AlertTypes Type { get; set; }
    }
}
