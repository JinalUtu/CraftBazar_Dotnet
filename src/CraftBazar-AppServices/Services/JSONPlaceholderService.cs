using System.Net.Http.Json;

public class JsonPlaceholderService : IJSONPlaceHolderService
{
    private readonly HttpClient _httpClient;

    public JsonPlaceholderService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    public async Task<string> GetUsersAsync()
    {
        return await _httpClient.GetStringAsync("https://jsonplaceholder.typicode.com/users");
    }

    public async Task<JsonUserRequestDto?> CreateUserAsync(JsonUserRequestDto user)
    {
        var response = await _httpClient.PostAsJsonAsync("https://jsonplaceholder.typicode.com/users", user);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonUserRequestDto>();
    }
    public async Task<JsonUserRequestDto?> UpdateUserAsync(int id, JsonUserRequestDto user)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"https://jsonplaceholder.typicode.com/users/{id}",
            user);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonUserRequestDto>();
    }

    public async Task<JsonUserRequestDto?> PatchUserAsync(int id, object update)
    {
        var response = await _httpClient.PatchAsJsonAsync(
            $"https://jsonplaceholder.typicode.com/users/{id}",
            update);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonUserRequestDto>();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync(
            $"https://jsonplaceholder.typicode.com/users/{id}");

        return response.IsSuccessStatusCode;
    }
}
