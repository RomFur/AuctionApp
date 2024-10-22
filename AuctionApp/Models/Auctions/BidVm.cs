using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class BidVm
{
    [ScaffoldColumn(false)]
    public int Id { get; set; }
    
    [Display(Name = "Bid placed by")]
    public string BidderId { get; set; }
    
    public double Amount { get; set; }
    
    [Display(Name = "Bid date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime TimePlaced { get; set; }

    public static BidVm FromBid(Bid bid)
    {
        return new BidVm()
        {
            Id = bid.Id,
            BidderId = bid.BidderId,
            Amount = bid.Amount,
            TimePlaced = bid.TimePlaced
        };
    }
}