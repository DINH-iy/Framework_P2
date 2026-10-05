using Microsoft.EntityFrameworkCore;
using Game.Domain.Entities;
using Game.Api.Features.Game;
using GameEntity = Game.Domain.Entities.Game;
namespace Game.Domain;

public class GameDbContext : DbContext
{
    public GameDbContext() { }
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Enemy> Enemies { get; set; }
    public DbSet<ApiRequestLog> ApiRequestLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skeleton>().HasData(new Skeleton { Id = 1 });
        modelBuilder.Entity<Goblin>().HasData(new Goblin { Id = 2 });
        modelBuilder.Entity<Orc>().HasData(new Orc { Id = 3 });
    }
}
