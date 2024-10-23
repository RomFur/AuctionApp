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

    public void CreateAuction(string itemName, string description, double startingPrice, DateTime endDate, string userName)
    {
        if (itemName == null) throw new DataException("Item name");
        if (itemName.Length > 128) throw new DataException("Item name too long");
        if (description == null || description.Length > 512) throw new DataException("Description");
        if (startingPrice < 1) throw new DataException("Starting price is missing, or has to be greater than 0");
        if (endDate < DateTime.Now || endDate == null) throw new DataException("Ending date invalid");
        if (userName == null) throw new DataException("User name missing");
        
        Auction auction = new Auction(itemName, description, startingPrice, endDate, userName);
        _auctionPersistence.SaveAuction(auction);
    }

    public List<Auction> GetAllByUserName(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAllByUserName(userName);
        return auctions;
    }
}