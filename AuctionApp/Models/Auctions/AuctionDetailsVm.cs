using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class AuctionDetailsVm
{
    [ScaffoldColumn(false)] 
    public int Id { get; set; }

    public string ItemName { get; set; }

    public string ItemDescription { get; set; }
    
    public double StartingPrice { get; set; }
    
    public double CurrentBid { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    [Display(Name = "Start Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Due Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime EndDate { get; set; }
    
    public List<BidVm> BidVms { get; set; } = new();

    public static AuctionDetailsVm FromAuction(Auction auction)
    {
        var detailsVM = new AuctionDetailsVm()
        {
            Id = auction.Id,
            ItemName = auction.ItemName,
            ItemDescription = auction.Description,
            StartingPrice = auction.StartingPrice,
            CurrentBid = auction.GetHighestBid()?.Amount ?? 0, 
            IsActive = auction.IsActive,
            StartDate = auction.StartDate,
            EndDate = auction.EndDate ?? DateTime.MinValue,
        };
        foreach (var bid in auction.Bids)
        {
            detailsVM.BidVms.Add(BidVm.FromBid(bid));
        }
        return detailsVM;
    }
}