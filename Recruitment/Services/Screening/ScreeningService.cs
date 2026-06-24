using System.Text;
using System.Text.Json;

namespace Recruitment.API.Services.Screening
{
    public interface IScreeningService
    {
        Task<ScreeningReport?> ScreenOneCvAsync(string cvPath, string requirements);
        Task<RankingResponse?> ScreenAllCvsAsync(List<string> cvPaths, string requirements, Dictionary<string, string> imena);
    }

    public class ScreeningService : IScreeningService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl = "http://localhost:8000";

        public ScreeningService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ScreeningReport?> ScreenOneCvAsync(string cvPath, string requirements)
        {
            var payload = new { cv_path = cvPath, requirements };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_pythonApiUrl}/screen", content);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ScreeningReport>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<RankingResponse?> ScreenAllCvsAsync(List<string> cvPaths, string requirements, Dictionary<string, string> imena)
        {
            var payload = new { cv_paths = cvPaths, requirements, imena };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_pythonApiUrl}/screen-all", content);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RankingResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
