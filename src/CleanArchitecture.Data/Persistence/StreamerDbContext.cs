using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class StreamerDbContext : DbContext
{
    private readonly IUserAccessorService _userAccessorService;
    public StreamerDbContext(DbContextOptions<StreamerDbContext> options
                            ,IUserAccessorService userAccessorService) : base(options)
    {
        _userAccessorService = userAccessorService;
    }
    public DbSet<Streamer> Streamers { get; set; }

    public DbSet<Video> Videos { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Director> Directors { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // base.OnConfiguring(optionsBuilder);
    //    optionsBuilder.UseSqlite(@"Data Source=../../db.sqlite;")
    //            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
    //            .EnableSensitiveDataLogging();
    //}

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userName = _userAccessorService.GetUserName();

        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy ??= userName;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy ??= userName;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy ??= userName;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges()
    {
        var userName = _userAccessorService.GetUserName();

        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy ??= userName;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy ??= userName;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy ??= userName;
                    break;
            }
        }
        return base.SaveChanges();
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
