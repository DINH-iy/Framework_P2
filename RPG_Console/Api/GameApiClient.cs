using System.Net.Http.Json;
using RPG_Console.Models;

namespace RPG_Console.Api;

public class GameApiClient
{
    private readonly HttpClient _httpClient;

    public GameApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GameState> StartGameAsync(string playerName)
    {
        var response = await _httpClient.PostAsJsonAsync("api/game", new { PlayerName = playerName });
        return await ReadGameStateAsync(response);
    }

    public async Task<List<GameSummary>> GetGamesAsync()
    {
        var response = await _httpClient.GetAsync("api/game");
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API returned {(int)response.StatusCode}: {message}");
        }

        return await response.Content.ReadFromJsonAsync<List<GameSummary>>() ?? new List<GameSummary>();
    }

    public async Task<GameState> GetGameAsync(int gameId)
    {
        var response = await _httpClient.GetAsync($"api/game/{gameId}");
        return await ReadGameStateAsync(response);
    }

    public async Task<GameState> MoveAsync(int gameId)
    {
        var response = await _httpClient.PostAsync($"api/game/{gameId}/move", null);
        return await ReadGameStateAsync(response);
    }

    public async Task<GameState> AttackAsync(int gameId, string attackType)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/game/{gameId}/attack",new { Type = attackType });
        return await ReadGameStateAsync(response);
    }

    public async Task<GameState> RunAsync(int gameId)
    {
        var response = await _httpClient.PostAsync($"api/game/{gameId}/run", null);
        return await ReadGameStateAsync(response);
    }

    private static async Task<GameState> ReadGameStateAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API returned {(int)response.StatusCode}: {message}");
        }

        return await response.Content.ReadFromJsonAsync<GameState>()
            ?? throw new InvalidOperationException("The API returned an empty game state.");
    }
}
