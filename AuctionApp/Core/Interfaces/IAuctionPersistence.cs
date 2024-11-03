using System;
using System.Collections.Generic;
using AuctionApp.Core;

namespace ProjectApp.Core.Interfaces
{
    public interface IAuctionPersistence
    {
        List<Auction> GetAllActiveAuctions();
        List<Auction> GetAllByUserName(string userName);
        Auction GetById(int id, string username);
        Auction GetById(int id);
        void SaveAuction(Auction auction);
        void SaveBid(int auctionId, Bid bid);
        List<Auction> GetByUserBid(string userName);
        List<Auction> GetAuctionsWon(string userName);
        void UpdateAuction(Auction auction);
        void DeleteAuction(int id);
    }
}