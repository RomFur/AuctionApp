using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Persistence;

public class AuctionDb
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(128)]
    public string ItemName { get; set; }
    
    [Required]
    [MaxLength(512)]
    public string Description { get; set; }
    
    [Required]
    public double StartingPrice { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    public List<BidDb> BidDbs { get; set; } = new List<BidDb>();
}