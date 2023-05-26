using EasMe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Infrastructure
{
    public class MarketDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(EasConfig.GetConnectionString("MARKET_DB"));
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
#endif
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasMany(x => x.Characters)
            //    .WithOne(x => x.User)
            //    .OnDelete(DeleteBehavior.ClientNoAction);

            //modelBuilder.Entity<Order>()
            //    .HasOne(x => x.Character)
            //    .WithMany(x => x.Orders)
            //    .OnDelete(DeleteBehavior.ClientNoAction);

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRequest> OrderRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<SecurityLog> SecurityLogs { get; set; }


    }
}