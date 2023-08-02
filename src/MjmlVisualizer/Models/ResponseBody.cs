using System.Text.Json.Serialization;

namespace MjmlVisualizer.Models
{
    public class ResponseBody
    {
        [JsonPropertyName("mjml")]
        public string MJML { get; set; } = string.Empty;

        [JsonPropertyName("html")]
        public string HTML { get; set; } = string.Empty;
    }
}
