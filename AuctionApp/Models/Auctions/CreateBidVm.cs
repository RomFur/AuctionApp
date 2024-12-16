using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

public class CreateBidVm
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than 0.")]
    public double Amount { get; set; }
    
    public int AuctionId { get; set; }
    
    public double StartingBid { get; set; }
}