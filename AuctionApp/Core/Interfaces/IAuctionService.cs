using AuctionApp.Core;

namespace ProjectApp.Core.Interfaces
{
    //Kan behöva Bidservice och IBidService...
    public interface IAuctionService
    {
        List<Auction> GetAllActiveAuctions();
        List<Auction> GetById(int id);
        List<Auction> GetByAllByUserName(string userName);
        void CreateAuction(string itemName, string description, decimal startingPrice, DateTime endDate, string userName);
        void CloseAuction(int auctionId);
    }
}