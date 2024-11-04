using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class AuctionVm
{
    [ScaffoldColumn(false)]
    public int Id { get; set; }
    
    public string ItemName { get; set; }
    
    public string ItemDescription { get; set; }
    
    [Display(Name = "Ends on")]
    [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd HH:mm}")]
    public DateTime EndDate { get; set; }
   
    public string AuctioneerId { get; set; }
    
    public bool IsActive { get; set; }

    public static AuctionVm FromAuction(Auction auction)
    {
        return new AuctionVm()
        {
            Id = auction.Id,
            ItemName = auction.ItemName,
            ItemDescription = auction.Description,
            EndDate = auction.EndDate,  // Use a default date if EndDate is null
            AuctioneerId = auction.UserName,
            IsActive = auction.IsActive,
        };
    }
}