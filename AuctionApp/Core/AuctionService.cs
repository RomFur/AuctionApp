using System.Data;
using ProjectApp.Core.Interfaces;

namespace AuctionApp.Core;

public class AuctionService : IAuctionService
{
    private readonly IAuctionPersistence _auctionPersistence;

    public AuctionService(IAuctionPersistence auctionPersistence)
    {
        _auctionPersistence = auctionPersistence;
    }

    
    public Auction GetById(int id, string userName)
    {
        Auction auction = _auctionPersistence.GetById(id, userName);
        if (auction == null) throw new DataException("Auction not found");
        return auction;
    }

    public List<Auction> GetAllActiveAuctions()
    {
        throw new NotImplementedException();
    }

    public List<Auction> GetAllInactiveAuctions()
    {
        throw new NotImplementedException();
    }

    public void CloseAuction(int auctionId)
    {
        throw new NotImplementedException();
    }

    public void CreateAuction(string itemName, string description, decimal startingPrice, DateTime endDate, string userName)
    {
        throw new NotImplementedException();
    }

    public List<Auction> GetAllByUserName(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAllByUserName(userName);
        return auctions;
    }
}