using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;

public class StreamerDbContext : DbContext
{
    public DbSet<Streamer> Streamers { get; set; }

    public DbSet<Video> Videos { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Director> Directors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(@"Data Source=../../db.sqlite;")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Streamer>()
                .HasMany(m => m.Videos)
                .WithOne(m => m.Streamer)
                .HasForeignKey(m => m.StreamerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Video>()
                .HasMany(m => m.Actors)
                .WithMany(m => m.Videos)
                .UsingEntity<VideoActor>(tb => tb.HasKey(va => new { va.ActorId, va.VideoId }));
    }
}
