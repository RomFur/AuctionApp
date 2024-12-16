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
    
    public void SaveAuction(Auction auction)
    {
        AuctionDb adb = _mapper.Map<AuctionDb>(auction);
        _dbContext.AuctionDbs.Add(adb);
        _dbContext.SaveChanges();
    }

    public void SaveBid(int auctionId, Bid bid)
    {
        BidDb bidDb = _mapper.Map<BidDb>(bid);
        
        bidDb.AuctionId = auctionId;
        
        if (bidDb.DateCreated == DateTime.MinValue)
        {
            bidDb.DateCreated = DateTime.Now;  
        }
        
        _dbContext.BidDbs.Add(bidDb);
        _dbContext.SaveChanges();
    }

    
    public void UpdateAuction(Auction auction)
    {
        AuctionDb auctionDb = _dbContext.AuctionDbs
            .FirstOrDefault(a => a.Id == auction.Id && a.UserName == auction.UserName);
        
        if (auctionDb == null)
        {
            throw new DataException("Auction not found.");
        }
        
        auctionDb.Description = auction.Description;
        
        _dbContext.AuctionDbs.Update(auctionDb);
        _dbContext.SaveChanges();
    }

    public Auction GetById(int id, string userName)
    {
    AuctionDb auctionDb = _dbContext.AuctionDbs
        .Where(a => a.Id == id && a.UserName == userName) 
        .Include(a => a.BidDbs)
        .AsNoTracking()  
        .FirstOrDefault();

    if (auctionDb == null) throw new DataException("Auction not found");

    Auction auction = _mapper.Map<Auction>(auctionDb);
    foreach (BidDb bidDb in auctionDb.BidDbs)
    {
        Bid bid = _mapper.Map<Bid>(bidDb);
        auction.AddBid(bid);
    }
    return auction;
    }

    public Auction GetById(int id)
    {
        AuctionDb auctionDb = _dbContext.AuctionDbs
            .Where(a => a.Id == id)
            .Include(a => a.BidDbs)
            .AsNoTracking()  
            .FirstOrDefault();

        if (auctionDb == null) throw new DataException("Auction not found");
        
        Auction auction = _mapper.Map<Auction>(auctionDb);
        
        foreach (BidDb bidDb in auctionDb.BidDbs)
        {
            Bid bid = new Bid(
                id: bidDb.Id,
                bidderId: bidDb.BidderId,
                amount: bidDb.Amount,
                timePlaced: bidDb.DateCreated 
            );

            auction.AddBid(bid);
        }
        return auction;
    }

    public List<Auction> GetAllActiveAuctions()
    {
        var activeAuctionDbs = _dbContext.AuctionDbs
            .Where(a => a.EndDate > DateTime.Now)  
            .OrderBy(a => a.EndDate)          
            .ToList();
        
        List<Auction> activeAuctions = new List<Auction>();
        foreach (var auctionDb in activeAuctionDbs)
        {
            var auction = _mapper.Map<Auction>(auctionDb);
            activeAuctions.Add(auction);
        }
    
        return activeAuctions;
    }

    public List<Auction> GetAuctionsWon(string userName)
    {
        var wonAuctionDbs = _dbContext.AuctionDbs
            .Where(a => a.EndDate <= DateTime.Now) 
            .Include(a => a.BidDbs)              
            .Where(a => a.BidDbs.Any())           // Ensure there is at least one bid
            .ToList()
            .Where(a => a.BidDbs.OrderByDescending(b => b.Amount).First().BidderId == userName) 
            .ToList();
        
        List<Auction> wonAuctions = new List<Auction>();
        foreach (var auctionDb in wonAuctionDbs)
        {
            var auction = _mapper.Map<Auction>(auctionDb);
            
            foreach (var bidDb in auctionDb.BidDbs)
            {
                var bid = _mapper.Map<Bid>(bidDb);
                auction.AddBid(bid);
            }

            wonAuctions.Add(auction);
        }

        return wonAuctions;
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

    public List<Auction> GetByUserBid(string userName)
    {
        var activeAuctionDbs = _dbContext.AuctionDbs
            .Where(a => a.EndDate > DateTime.Now) 
            .Include(a => a.BidDbs) 
            .Where(a => a.BidDbs.Any(b => b.BidderId == userName))
            .ToList();
        
        List<Auction> auctions = activeAuctionDbs.Select(auctionDb =>
        {
            var auction = _mapper.Map<Auction>(auctionDb);
            
            foreach (var bidDb in auctionDb.BidDbs.Where(b => b.BidderId == userName))
            {
                var bid = _mapper.Map<Bid>(bidDb);
                auction.PlaceBid(bid);
            }

            return auction;
        }).ToList();

        return auctions;
    }

}
