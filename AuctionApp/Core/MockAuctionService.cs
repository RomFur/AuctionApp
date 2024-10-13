using ProjectApp.Core.Interfaces;

namespace AuctionApp.Core;

public class MockAuctionService : IAuctionService
{
    // Fetch all active auctions
    public List<Auction> GetAllActiveAuctions()
    {
        return _auctions.FindAll(a => a.EndDate > DateTime.Now);  // Fetch auctions that are still active
    }

    public List<Auction> GetById(int id)
    {
        return _auctions.FindAll(a => a.Id == id);
    }

    public Auction GetAuctionById(int id, string userId)
    {
        return _auctions.Find(a => a.Id == id && a.AuctioneerId == userId);
    }

    public void CreateAuction(string itemName, string description, decimal startingPrice, DateTime endDate, string userName)
    {
        throw new NotImplementedException();
    }

    public void CloseAuction(int auctionId)
    {
        throw new NotImplementedException();
    }

    // Static list to hold auction data
    private static readonly List<Auction> _auctions = new();

    static MockAuctionService()
    {
        // Add sample auctions to the list
        Auction a1 = new Auction(1, "some thing", "description of the thing", 10, DateTime.Now, DateTime.Now.AddHours(1), "rlfurman@kth.se");
        Auction a2 = new Auction(2, "another thing", "description of another thing", 15, DateTime.Now, DateTime.Now.AddHours(2), "rlfurman@kth.se");
        _auctions.Add(a1);
        _auctions.Add(a2);
    }
}
