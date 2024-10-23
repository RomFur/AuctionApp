using System.Data;
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
            List<Auction> auctions = _auctionService.GetAllByUserName("rlfurman@kth.se");  // Get all active auctions
            List<AuctionVm> auctionsVms = new List<AuctionVm>();
            foreach (var auction in auctions)
            {
                auctionsVms.Add(AuctionVm.FromAuction(auction));
            }
            return View(auctionsVms);  // Pass the list of AuctionVm to the view
        }

        // GET : AuctionsController/Details
        public ActionResult Details(int id)
        {
            
                Auction auction = _auctionService.GetById(id, "rlfurman@kth.se"); // current user
                if (auction == null) return BadRequest("Might be null"); // HTTP 400

                AuctionDetailsVm detailsVm = AuctionDetailsVm.FromAuction(auction);
                return View(detailsVm);
            
           
        }
        
        // GET : AuctionsController/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST : AuctionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAuctionVm createAuctionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string itemName = createAuctionVm.ItemName;
                    string description = createAuctionVm.Description;
                    double startingPrice = createAuctionVm.StartingPrice;
                    DateTime endDate = createAuctionVm.EndDate;
                    string userName = User.Identity.Name;
                    
                    _auctionService.CreateAuction(itemName, description, startingPrice, endDate, userName);
                    return RedirectToAction("Index");
                }
                return View(createAuctionVm);
            }
            catch (DataException e)
            {
                return View(createAuctionVm);
            }
        }
        
        // Other actions...
    }

}
