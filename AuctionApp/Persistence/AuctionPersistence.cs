using System.Data;
using AuctionApp.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using ProjectApp.Core.Interfaces;

namespace AuctionApp.Persistence;

public class AuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;

    public AuctionPersistence(AuctionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void DeleteAuction(int id)
    {
        throw new NotImplementedException();
    }

    public void SaveAuction(Auction auction)
    {
        AuctionDb adb = _mapper.Map<AuctionDb>(auction);
        _dbContext.AuctionDbs.Add(adb);
        _dbContext.SaveChanges();
    }

    public void SaveBid(int auctionId, Bid bid)
    {
        // Map the Bid domain model to the BidDb entity
        BidDb bidDb = _mapper.Map<BidDb>(bid);

        // Set the AuctionId on the BidDb to ensure it's linked to the correct auction
        bidDb.AuctionId = auctionId;

        // Ensure DateCreated is only set if it's the minimum value
        if (bidDb.DateCreated == DateTime.MinValue)
        {
            bidDb.DateCreated = DateTime.Now;  // Set to now only if it wasn’t set earlier
        }

        // Add the bid to the DbContext
        _dbContext.BidDbs.Add(bidDb);

        // Commit the changes to the database
        _dbContext.SaveChanges();
    }

    
    public void UpdateAuction(Auction auction)
    {
        // Retrieve the auction entity from the database
        AuctionDb auctionDb = _dbContext.AuctionDbs
            .FirstOrDefault(a => a.Id == auction.Id && a.UserName == auction.UserName);
    
        // Check if auction exists
        if (auctionDb == null)
        {
            throw new DataException("Auction not found.");
        }
    
        // Update the description (assuming only description is allowed to be updated)
        auctionDb.Description = auction.Description;

        // Save changes back to the database
        _dbContext.AuctionDbs.Update(auctionDb);
        _dbContext.SaveChanges();
    }

    public Auction GetById(int id, string username)
    {
        // Use AsNoTracking to avoid Entity Framework tracking changes in retrieved entities
    AuctionDb auctionDb = _dbContext.AuctionDbs
        .Where(a => a.Id == id)
        .Include(a => a.BidDbs)
        .AsNoTracking()  // Disable tracking for read-only data
        .FirstOrDefault();

    if (auctionDb == null) throw new DataException("Auction not found");

    Auction auction = _mapper.Map<Auction>(auctionDb);
    foreach (BidDb bidDb in auctionDb.BidDbs)
    {
        Bid bid = _mapper.Map<Bid>(bidDb);
        auction.PlaceBid(bid);
    }
    return auction;
    }

    public Auction GetById(int id)
    {
        // Retrieve auction and bids with AsNoTracking for read-only operation
        AuctionDb auctionDb = _dbContext.AuctionDbs
            .Where(a => a.Id == id)
            .Include(a => a.BidDbs)
            .AsNoTracking()  
            .FirstOrDefault();

        if (auctionDb == null) throw new DataException("Auction not found");

        // Map AuctionDb to Auction
        Auction auction = _mapper.Map<Auction>(auctionDb);

        // Explicitly map BidDb to Bid and prevent resetting TimePlaced
        foreach (BidDb bidDb in auctionDb.BidDbs)
        {
            // Map BidDb to Bid, and ensure that TimePlaced reflects DateCreated accurately
            Bid bid = new Bid(
                id: bidDb.Id,
                bidderId: bidDb.BidderId,
                amount: bidDb.Amount,
                timePlaced: bidDb.DateCreated  // Set TimePlaced to DateCreated on the first mapping
            );

            auction.PlaceBid(bid);
        }
        return auction;
    }

    public List<Auction> GetAllActiveAuctions()
    {
        // Fetch auctions with EndDate in the future, sorted by EndDate
        var activeAuctionDbs = _dbContext.AuctionDbs
            .Where(a => a.EndDate > DateTime.Now)  // Filter for future auctions
            .OrderBy(a => a.EndDate)               // Sort by EndDate
            .ToList();
    
        // Map to domain model
        List<Auction> activeAuctions = new List<Auction>();
        foreach (var auctionDb in activeAuctionDbs)
        {
            var auction = _mapper.Map<Auction>(auctionDb);
            activeAuctions.Add(auction);
        }
    
        return activeAuctions;
    }

    public List<Auction> GetAllByUserName(string userName)
    {
        var auctioDbs = _dbContext.AuctionDbs
            .Where(a => a.UserName == userName)
            .ToList();
        
        List<Auction> result = new List<Auction>();
        foreach (AuctionDb adb in auctioDbs)
        {
            Auction auction = _mapper.Map<Auction>(adb);
            result.Add(auction);
        }
        return result;
    }
}
