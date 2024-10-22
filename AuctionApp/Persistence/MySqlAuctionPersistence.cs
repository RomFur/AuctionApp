using System.Data;
using AuctionApp.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using ProjectApp.Core.Interfaces;

namespace AuctionApp.Persistence;

public class MySqlAuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;

    public MySqlAuctionPersistence(AuctionDbContext dbContext, IMapper mapper)
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
        throw new NotImplementedException();
    }

    public void UpdateAuction(Auction auction)
    {
        throw new NotImplementedException();
    }

    public Auction GetById(int id, string username)
    {
        AuctionDb auctionDb = _dbContext.AuctionDbs
            .Where(a => a.Id == id && a.UserName.Equals(username))
            .Include(a => a.BidDbs)
            .FirstOrDefault(); // null if not found!
        
        if (auctionDb == null) throw new DataException("Auction not found");
        
        Auction auction = _mapper.Map<Auction>(auctionDb);
        foreach (BidDb bidDb in auctionDb.BidDbs)
        {
            Bid bid = _mapper.Map<Bid>(bidDb);
            auction.PlaceBid(bid);
        }
        return auction;
    }

    public List<Auction> GetAllActiveAuctions()
    {
        throw new NotImplementedException();
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
