using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Core;

namespace AuctionApp.Persistence;

public class BidDb
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string BidderId { get; set; }
    
    [Required]
    public double Amount { get; set; }
    
    [ForeignKey("AuctionId")]
    public AuctionDb AuctionDb { get; set; }
    
    public int AuctionId { get; set; }
}