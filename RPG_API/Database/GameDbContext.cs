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
    public DbSet<Item> Items { get; set; }
    public DbSet<PlayerItem> PlayerItems { get; set; }
    public DbSet<ApiRequestLog> ApiRequestLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skeleton>().HasData(new Skeleton { Id = 1 });
        modelBuilder.Entity<Goblin>().HasData(new Goblin { Id = 2 });
        modelBuilder.Entity<Orc>().HasData(new Orc { Id = 3 });

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Health Potion", Type = ItemType.Healing, Value = 25, Speed = 0 },
            new Item { Id = 2, Name = "Iron Sword", Type = ItemType.Equipment, Value = 4, Speed = 3 });
    }
}
