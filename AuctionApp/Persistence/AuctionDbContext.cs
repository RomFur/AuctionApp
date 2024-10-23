using AuctionApp.Core;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Persistence;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }
    
    public DbSet<BidDb> BidDbs { get; set; }
    public DbSet<AuctionDb> AuctionDbs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AuctionDb adb = new AuctionDb()
        {
            Id = -1,
            Description = "Auction Db Description",
            EndDate = new DateTime(2025, 12, 31),
            StartingPrice = 1.00,
            ItemName = "Test item",
            StartDate = DateTime.Now,
            UserName = "rlfurman@kth.se",
            BidDbs = new List<BidDb>()
        };
        modelBuilder.Entity<AuctionDb>().HasData(adb);

        BidDb bdb1 = new BidDb()
        {
            Id = -1,
            Amount = 2.50,
            BidderId = "rlfurman@hotmail.com",
            DateCreated = DateTime.Now,
            AuctionId = -1,
        };
        BidDb bdb2 = new BidDb()
        {
            Id = -2,
            Amount = 1.28,
            BidderId = "rlfurman@hotmail.com",
            DateCreated = DateTime.Now,
            AuctionId = -1,
        };
        modelBuilder.Entity<BidDb>().HasData(bdb1);
        modelBuilder.Entity<BidDb>().HasData(bdb2);
    }
}