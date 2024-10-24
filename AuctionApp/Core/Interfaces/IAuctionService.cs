using AuctionApp.Core;

namespace ProjectApp.Core.Interfaces
{
    //Kan behöva Bidservice och IBidService...
    public interface IAuctionService
    {
        List<Auction> GetAllActiveAuctions();
        Auction GetById(int id, string userName);
        List<Auction> GetAllByUserName(string userName);
        void CreateAuction(string itemName, string description, double startingPrice, DateTime endDate, string userName);
        void UpdateAuction(Auction auction);
        void CloseAuction(int auctionId);
    }
}