namespace AuctionApp.Core
{
    public class Auction
    {
        public int Id { get; set; }  

        public string ItemName { get; set; }  
        public string Description { get; set; }  
        public double StartingPrice { get; set; }  
        public string UserName { get; set; }  // User who is auctioning the item.
        public DateTime StartDate { get; set; }  //behövs?
        public DateTime? EndDate { get; private set; }  

        private List<Bid> _bids = new List<Bid>();  
        public IEnumerable<Bid> Bids => _bids.AsReadOnly();  // Read-Only

        public bool IsActive => DateTime.Now <= EndDate;
        
        public Auction(string itemName, string description, double startingPrice, DateTime endDate, string userName)
        {
            ItemName = itemName;
            Description = description;
            StartingPrice = startingPrice;
            StartDate = DateTime.Now;
            EndDate = endDate;
            UserName = userName;
        }

        public Auction(int id, string itemName, string description, double startingPrice, DateTime startDate, DateTime? endDate, string userName)
        {
            Id = id;
            ItemName = itemName;
            Description = description;
            StartingPrice = startingPrice;
            StartDate = startDate;
            EndDate = endDate;
            UserName = userName;
        }

        public Auction() { }
        
        public void PlaceBid(Bid newBid)
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is closed.");

            if (_bids.Count > 0 && newBid.Amount <= _bids.Max(b => b.Amount))
                throw new InvalidOperationException("Bid must be higher than the current highest bid.");

            if (newBid.Amount <= StartingPrice)
                throw new InvalidOperationException("Bid must be higher than the starting price.");

            _bids.Add(newBid);
        }

      /*  //GET: Highest Bid (Bid?)
        public Bid? GetHighestBid()
        {
            return _bids.OrderByDescending(b => b.Amount).FirstOrDefault();
        } */
        
        //GET: Highest Bid
        public Bid GetHighestBid()
        {
            return _bids.OrderByDescending(b => b.Amount).FirstOrDefault();
        }

        //Stäng Auction
        public void CloseAuction()
        {
            EndDate = DateTime.Now;
        }

        public override string ToString()
        {
            var highestBid = GetHighestBid();
            return $"{Id}: {ItemName} - Description: {Description} - Starting Price: {StartingPrice:C} - Highest bid: {(highestBid != null ? highestBid.Amount.ToString("C") : "None")} - Auction active: {IsActive}";
        }
    }
}

