using AuctionApp.Core;
using AuctionApp.Models.Auctions;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Core.Interfaces;

namespace AuctionApp.Controllers
{
    public class AuctionsController : Controller
    {
        private IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        // GET: AuctionsController
        public ActionResult Index()
        {
            List<Auction> auctions = _auctionService.GetById(1);  // Get all active auctions
            List<AuctionVm> auctionsVms = new List<AuctionVm>();
            foreach (var auction in auctions)
            {
                auctionsVms.Add(AuctionVm.FromAuction(auction));
            }
            return View(auctionsVms);  // Pass the list of AuctionVm to the view
        }

        // Other actions...
    }

}
