using System;
using System.Collections.Generic;
using AuctionApp.Core;

namespace ProjectApp.Core.Interfaces
{
    public interface IAuctionPersistence
    {
        List<Auction> GetAllActiveAuctions();
        Auction GetById(int id);
        void AddAuction(Auction auction);
        void UpdateAuction(Auction auction);
        void DeleteAuction(int id);
    }
}