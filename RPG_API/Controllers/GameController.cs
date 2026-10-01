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
    public async Task<ActionResult<GameResponse>> Move(int id, MoveRequest request)
    {
        var game = await _gameService.MoveAsync(id, request.Direction);
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

    [HttpPost("{id}/items/{slot}/use")]
    public async Task<ActionResult<GameResponse>> UseItem(int id, int slot)
    {
        var game = await _gameService.UseItemAsync(id, slot);
        return Ok(game);
    }

    [HttpPost("{id}/equip")]
    public async Task<ActionResult<GameResponse>> EquipItem(int id, EquipItemRequest request)
    {
        var game = await _gameService.EquipItemAsync(id, request.Selection);
        return Ok(game);
    }

    [HttpGet("{id}/equipable-items")]
    public async Task<ActionResult<List<ItemResponse>>> GetEquipableItems(int id)
    {
        var items = await _gameService.GetEquipableItemsAsync(id);
        return Ok(items);
    }
}