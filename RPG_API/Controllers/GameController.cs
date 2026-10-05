using Microsoft.AspNetCore.Mvc;
using Game.Api.Models;
using Game.Api.Services;
namespace Game.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public async Task<ActionResult<GameResponse>> Start(StartGameRequest request)
    {
        var game = await _gameService.StartAsync(request);
        return Ok(game);
    }

    [HttpGet]
    public async Task<ActionResult<List<GameSummaryResponse>>> GetGames()
    {
        var games = await _gameService.GetGamesAsync();
        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameResponse>> Get(int id)
    {
        var game = await _gameService.GetAsync(id);
        return Ok(game);
    }

    [HttpPost("{id}/move")]
    public async Task<ActionResult<GameResponse>> Move(int id)
    {
        var game = await _gameService.MoveAsync(id);
        return Ok(game);
    }

    [HttpPost("{id}/attack")]
    public async Task<ActionResult<GameResponse>> Attack(int id, AttackRequest request)
    {
        var game = await _gameService.AttackAsync(id, request.Type);
        return Ok(game);
    }

    [HttpPost("{id}/run")]
    public async Task<ActionResult<GameResponse>> Run(int id)
    {
        var game = await _gameService.RunAsync(id);
        return Ok(game);
    }
}