using GameZone.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Data
{
    public class GameZoneDbContext : IdentityDbContext
    {
        public GameZoneDbContext(DbContextOptions<GameZoneDbContext> options)
            : base(options)
        {
        }


        public DbSet<Genre> Genres { get; set; } 
        public DbSet<Game> Games { get; set; } 
        public DbSet<GamerGame> GamersGames { get; set; } 


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<GamerGame>()
                .HasKey(e => new { e.GamerId, e.GameId });

            builder.Entity<GamerGame>()
                .HasOne(e => e.Gamer)
                .WithMany()
                .HasForeignKey(e => e.GamerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GamerGame>()
                .HasOne(e => e.Game)
                .WithMany(e => e.GamersGames)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.NoAction);

            

            builder
                .Entity<Genre>()
                .HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Adventure" },
                new Genre { Id = 3, Name = "Fighting" },
                new Genre { Id = 4, Name = "Sports" },
                new Genre { Id = 5, Name = "Racing" },
                new Genre { Id = 6, Name = "Strategy" });

            base.OnModelCreating(builder);
        }
    }
}
