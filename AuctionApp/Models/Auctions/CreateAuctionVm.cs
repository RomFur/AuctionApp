using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

public class CreateAuctionVm
{
    [Required]
    [StringLength(128, ErrorMessage = "Max length 128 characters")]
    public string? ItemName { get; set; }
    
    [Required]
    [StringLength(512, ErrorMessage = "Max length 512 characters")]
    public string? Description { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public double StartingPrice { get; set; }
    
    [Required]
    [FutureDate(ErrorMessage = "End Date must be in the future")]
    public DateTime EndDate { get; set; }
}