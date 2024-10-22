using ProjectApp.Core.Interfaces;

namespace AuctionApp.Core;

public class AuctionService : IAuctionService
{
    private readonly IAuctionPersistence _auctionPersistence;

    public AuctionService(IAuctionPersistence auctionPersistence)
    {
        _auctionPersistence = auctionPersistence;
    }

    
    public List<Auction> GetById(int id)
    {
        throw new NotImplementedException();
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

    public List<Auction> GetByAllByUserName(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAllByUserName(userName);
        return auctions;
    }
}