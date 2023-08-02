namespace MjmlVisualizer.Models
{
    public class RequestBody
    {
        public RequestBody(string mjml)
        {
            MJML = mjml;
        }

        public string MJML { get; set; } = string.Empty;
    }
}
