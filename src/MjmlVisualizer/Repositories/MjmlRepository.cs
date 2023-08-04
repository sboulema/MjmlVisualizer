using MjmlVisualizer.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MjmlVisualizer.Repositories
{
    public static class MjmlRepository
    {
        private const string _apiURL = "https://api.mjml.io/v1/render";
        private static readonly HttpClient _httpClient;

        static MjmlRepository()
        {
            var userName = Properties.Settings.Default.MJMLUserName;
            var password = Properties.Settings.Default.MJMLPassword;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}")));
        }

        public static async Task<ResponseBody> GenerateHTML(string mjml)
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(new RequestBody(mjml)));

            var response = await _httpClient.PostAsync(_apiURL, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseBody>(json);

            return result;
        }
    }
}
