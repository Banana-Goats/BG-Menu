using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class YeastarApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public YeastarApiService(string baseUrl, string username, string password)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();

        // Set up Basic Authentication
        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        // If using API Key instead, uncomment the following line and comment out the Basic Auth lines
        // _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_API_KEY");
    }

    public async Task<string> GetCustomTemplateAsync(string templateName)
    {
        string endpoint = $"/resource_repository/custom_templates/{templateName}";
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + endpoint);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new HttpRequestException($"Failed to fetch template. Status Code: {response.StatusCode}");
        }
    }
}
