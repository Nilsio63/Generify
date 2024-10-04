using Generify.Models.Management;
using Generify.Models.Playlists;
using Microsoft.EntityFrameworkCore;

namespace Generify.Repositories;

public class GenerifyDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<PlaylistDefinition> PlaylistDefinitions { get; set; }
    public DbSet<PlaylistSource> PlaylistSources { get; set; }
    public DbSet<OrderInstruction> OrderInstructions { get; set; }

    public GenerifyDataContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Id).ValueGeneratedOnAdd();
            e.Property(o => o.SpotifyId).IsRequired().HasMaxLength(255);
            e.Property(o => o.RefreshToken).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<PlaylistDefinition>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Id).ValueGeneratedOnAdd();
            e.Property(o => o.TargetPlaylistId).IsRequired().HasMaxLength(255);
            e.Property(o => o.UserId).IsRequired();

            e.HasOne(o => o.User).WithMany(o => o.PlaylistDefinitions).HasForeignKey(o => o.UserId).IsRequired();
        });

        modelBuilder.Entity<PlaylistSource>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Id).ValueGeneratedOnAdd();
            e.Property(o => o.SourceId).IsRequired().HasMaxLength(255);
            e.Property(o => o.SourceType).IsRequired();
            e.Property(o => o.InclusionType).IsRequired();

            e.HasOne(o => o.PlaylistDefinition).WithMany(o => o.PlaylistSources).IsRequired();
        });

        modelBuilder.Entity<OrderInstruction>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Id).ValueGeneratedOnAdd();
            e.Property(o => o.OrderType).IsRequired();
            e.Property(o => o.OrderDirection).IsRequired();

            e.HasOne(o => o.PlaylistDefinition).WithMany(o => o.OrderInstructions).IsRequired();
        });
    }
}
