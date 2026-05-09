using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TicketIT.Web.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("TicketITAPI");
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = CreateClient();
        var response = await client.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode) return default;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<(bool Success, T? Data, string? Error)> PostAsync<T>(string endpoint, object body)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<T>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return (true, data, null);
        }

        return (false, default, responseJson);
    }

    public async Task<(bool Success, string? Error)> PatchAsync(string endpoint, object body)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(endpoint, content);
        return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync());
    }
}