using System;

namespace AuctionApp.Core
{
    public class Bid
    {
        public int Id { get; set; }  

        public string BidderId { get; set; }  
        public double Amount { get; set; }  
        
        private DateTime _timePlaced;
        public DateTime TimePlaced => _timePlaced;  
        
        public Bid(string bidderId, double amount)
        {
            BidderId = bidderId;
            Amount = amount;
            _timePlaced = DateTime.Now;  
        }
        
        public Bid(int id, string bidderId, double amount, DateTime timePlaced)
        {
            Id = id;
            BidderId = bidderId;
            Amount = amount;
            _timePlaced = timePlaced;
        }

        public Bid() { }

        public override string ToString()
        {
            return $"{Id}: {BidderId} bid ${Amount} at {TimePlaced}";
        }
    }
}