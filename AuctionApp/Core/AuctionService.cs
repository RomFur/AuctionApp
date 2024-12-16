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
    public Auction GetById(int id)
    {
        Auction auction = _auctionPersistence.GetById(id);
        if (auction == null) throw new DataException("Auction not found");
        return auction;
    }

    public List<Auction> GetAllActiveAuctions()
    {
        List<Auction> auctions = _auctionPersistence.GetAllActiveAuctions();
        return auctions;
    }

    public List<Auction> GetByUserBid(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetByUserBid(userName);
        return auctions;
    }

    public List<Auction> GetAuctionsWon(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAuctionsWon(userName);
        return auctions;
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
    
    public void PlaceBid(int auctionId, double bidAmount, string bidderId)
    {
        Auction auction = _auctionPersistence.GetById(auctionId);
        
        if (auction == null)
        {
            throw new DataException("Auction not found.");
        }

        // Check if bidder is the owner
        if (auction.UserName == bidderId)
        {
            throw new InvalidOperationException("You cannot place a bid on your own auction.");
        }
        
        double minBidAmount = auction.Bids.Any() 
            ? auction.GetHighestBid().Amount + 1   // If bids exist, minimum bid is highest bid + 1
            : auction.StartingPrice;               // Otherwise, minimum bid is the starting price
        
        if (bidAmount < minBidAmount)
        {
            throw new InvalidOperationException($"Bid amount must be at least {minBidAmount}.");
        }
        
        var bid = new Bid(bidderId, bidAmount);
        auction.PlaceBid(bid);
        _auctionPersistence.SaveBid(auctionId, bid);
    }


    public void UpdateAuction(Auction auction)
    {
        if (auction == null) throw new DataException("Auction cannot be null");
        
        if (auction.Description == null || auction.Description.Length > 512)
            throw new DataException("Invalid description. It cannot be null or exceed 512 characters");
        
        _auctionPersistence.UpdateAuction(auction);  
    }
    
    public List<Auction> GetAllByUserName(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAllByUserName(userName);
        return auctions;
    }
}