using CoBetsDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace CoBetsDatabase
{
    public class CoBetsDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        public CoBetsDbContext(DbContextOptions<CoBetsDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("players");
                entity.HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId);
                entity.HasData(
                    new Player(1, 1, "Aboba", 20, 3)
                );
            });
            
            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("team");
                entity.HasData(
                    new Team(1, "Aboba Team")
                );
            });
        }
    }
}
