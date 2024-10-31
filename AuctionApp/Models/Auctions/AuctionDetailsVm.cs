using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class AuctionDetailsVm
{
    [ScaffoldColumn(false)] 
    public int Id { get; set; }
    
    [Display(Name = "Item Name")]
    public string ItemName { get; set; }
    [Display(Name = "Description")]

    public string ItemDescription { get; set; }

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

