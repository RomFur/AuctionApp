using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class AuctionVm
{
    [ScaffoldColumn(false)]
    public int Id { get; set; }
    
    public string ItemName { get; set; }
    
    public string ItemDescription { get; set; }
    
    [Display(Name = "Created Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy_mm_dd HH:mm}")]
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
            EndDate = auction.EndDate ?? DateTime.MinValue,  // Use a default date if EndDate is null
            AuctioneerId = auction.AuctioneerId,
            IsActive = auction.IsActive,
        };
    }
}