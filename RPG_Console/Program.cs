using RPG_Console;
using RPG_Console.Api;

using var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5265/")
};

var game = new ConsoleGame(new GameApiClient(httpClient));
await game.RunAsync();
