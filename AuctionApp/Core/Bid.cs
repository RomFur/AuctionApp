using System;

namespace AuctionApp.Core
{
    public class Bid
    {
        public int Id { get; set; }  

        public string BidderId { get; set; }  
        public decimal Amount { get; set; }  
        
        private DateTime _timePlaced;
        public DateTime TimePlaced => _timePlaced;  
        
        public Bid(string bidderId, decimal amount)
        {
            BidderId = bidderId;
            Amount = amount;
            _timePlaced = DateTime.Now;  
        }
        
        public Bid(int id, string bidderId, decimal amount, DateTime timePlaced)
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