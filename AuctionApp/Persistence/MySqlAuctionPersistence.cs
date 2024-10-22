using AuctionApp.Core;
using AutoMapper;
using MySql.Data.MySqlClient;
using ProjectApp.Core.Interfaces;

namespace AuctionApp.Persistence;

public class MySqlAuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;

    public MySqlAuctionPersistence(AuctionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void DeleteAuction(int id)
    {
        throw new NotImplementedException();
    }

    public void SaveAuction(Auction auction)
    {
        throw new NotImplementedException();
    }

    public void UpdateAuction(Auction auction)
    {
        throw new NotImplementedException();
    }

    public Auction GetById(int id, string username)
    {
        throw new NotImplementedException();
    }

    public List<Auction> GetAllActiveAuctions()
    {
        throw new NotImplementedException();
    }

    public List<Auction> GetAllByUserName(string userName)
    {
        var auctioDbs = _dbContext.AuctionDbs
            .Where(a => a.UserName == userName)
            .ToList();
        
        List<Auction> result = new List<Auction>();
        foreach (AuctionDb adb in auctioDbs)
        {
            Auction auction = _mapper.Map<Auction>(adb);
            result.Add(auction);
        }
        return result;
    }
}
