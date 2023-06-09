using EasMe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewWorldMarket.Entities;

namespace NewWorldMarket.Infrastructure;

public class MarketDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderRequest> OrderRequests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BlockedUser> BlockedUsers { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<OrderReport> OrderReports { get; set; }
    public DbSet<BlockedIpAddress> BlockedIpAddresses { get; set; }
    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

    public static bool EnsureCreated()
    {
        using var context = new MarketDbContext();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
        return true;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(EasConfig.GetConnectionString("MARKET_DB"));
#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Order>()
        //    .Navigation(x => x.Character)
        //    .AutoInclude();

        //modelBuilder.Entity<User>()
        //    .Navigation(x => x.Characters)
        //    .AutoInclude();
        //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        //{
        //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
        //}
        //modelBuilder.Entity<User>()
        //    .HasMany(x => x.Characters)
        //    .WithOne(x => x.User)
        //    .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<Character>()
        //    .HasMany(x => x.Orders)
        //    .WithOne()
        //    .HasForeignKey(x => x.CharacterGuid)
        //    .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<Order>()
        //    .HasOne(x => x.Character)
        //    .WithMany()
        //    .HasForeignKey(x => x.CharacterGuid)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}