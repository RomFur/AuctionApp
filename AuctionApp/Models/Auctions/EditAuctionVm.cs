using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

public class EditAuctionVm
{
    [Required]
    [StringLength(512, ErrorMessage = "Max length 512 characters")]
    public string Description { get; set; }
}