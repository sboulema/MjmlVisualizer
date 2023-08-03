using MjmlVisualizer.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MjmlVisualizer.Repositories
{
    public class MjmlRepository
    {
        private const string _apiURL = "https://api.mjml.io/v1/render";
        private const string _userName = "c4303cb3-7710-417d-9bc7-da57885b3432";
        private const string _password = "acaca040-7197-4450-b057-5bb3465f3459";
        private readonly HttpClient _httpClient;

        public MjmlRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userName}:{_password}")));
        }

        public ResponseBody GenerateHTML(string mjml)
        {
            var requestBody = new StringContent(JsonSerializer.Serialize(new RequestBody(mjml)));

            var response = _httpClient.PostAsync(_apiURL, requestBody).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var stream = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<ResponseBody>(stream);

            return result;
        }
    }
}
