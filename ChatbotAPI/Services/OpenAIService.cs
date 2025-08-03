using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatbotAPI.Services
{
    public class OpenAIService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _http;

        public OpenAIService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"] ?? throw new ArgumentNullException("OpenAI:ApiKey not set");
            _model = config["OpenAI:Model"] ?? "gpt-3.5-turbo";
            _http = new HttpClient();
        }

        public async Task<string> GetResponseAsync(string message)
        {
            var payload = new
            {
                model = _model,
                messages = new[] {
                    new { role = "user", content = message }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error from OpenAI: {response.StatusCode} - {json}";
            }

            var doc = JsonDocument.Parse(json);
            var content = doc.RootElement
                             .GetProperty("choices")[0]
                             .GetProperty("message")
                             .GetProperty("content")
                             .GetString();

            return content ?? "No response from OpenAI.";
        }
    }
}
