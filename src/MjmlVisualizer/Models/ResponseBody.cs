using Newtonsoft.Json;

namespace MjmlVisualizer.Models
{
    public class ResponseBody
    {
        [JsonProperty("mjml")]
        public string MJML { get; set; } = string.Empty;

        [JsonProperty("html")]
        public string HTML { get; set; } = string.Empty;
    }
}
