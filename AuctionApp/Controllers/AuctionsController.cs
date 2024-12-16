using System.Data;
using AuctionApp.Core;
using AuctionApp.Models.Auctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Core.Interfaces;

namespace AuctionApp.Controllers
{
    [Authorize]
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
            List<Auction> auctions = _auctionService.GetAllByUserName(User.Identity.Name); 
            List<AuctionVm> auctionsVms = new List<AuctionVm>();
            foreach (var auction in auctions)
            {
                auctionsVms.Add(AuctionVm.FromAuction(auction));
            }
            return View(auctionsVms);  
        }
        
        // GET: AuctionsController/Active
        public ActionResult Active()
        {
            List<Auction> activeAuctions = _auctionService.GetAllActiveAuctions();
            
            List<AuctionVm> activeAuctionVms = new List<AuctionVm>();
            foreach (var auction in activeAuctions)
            {
                activeAuctionVms.Add(AuctionVm.FromAuction(auction));
            }
            return View(activeAuctionVms);
        }

        public ActionResult ByBid()
        {
            List<Auction> auctionsWithBid = _auctionService.GetByUserBid(User.Identity.Name);
            List<AuctionVm> activeAuctionVms = new List<AuctionVm>();
            foreach (var auction in auctionsWithBid)
            {
                activeAuctionVms.Add(AuctionVm.FromAuction(auction));
            }
            return View(activeAuctionVms);
        }

        public ActionResult AuctionsWon()
        {
            List<Auction> auctionsWon = _auctionService.GetAuctionsWon(User.Identity.Name);
            
            List<AuctionVm> auctionsVms = new List<AuctionVm>();
            foreach (var auction in auctionsWon)
            {
                auctionsVms.Add(AuctionVm.FromAuction(auction));
            }
            
            return View(auctionsVms);
        }

        // GET : AuctionsController/Details
        public ActionResult Details(int id)
        {
          var auction = _auctionService.GetById(id);
    
          if (auction == null)
          {
              return NotFound();
          }

          var viewModel = new AuctionDetailsVm
          {
              Id = auction.Id,
              ItemName = auction.ItemName,
              ItemDescription = auction.Description,
              Auctioneer = auction.UserName,
              StartingPrice = auction.StartingPrice,
              IsActive = auction.IsActive,
              StartDate = auction.StartDate,
              EndDate = auction.EndDate,
              BidVms = auction.Bids
                  .OrderByDescending(b => b.Amount)  
                  .Select(bid => BidVm.FromBid(bid))
                  .ToList()
          };

          return View(viewModel);
            
           
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
        
        
        // GET : AuctionsController/CreateBid
        public ActionResult CreateBid(int id)
        {
            Auction auction = _auctionService.GetById(id);

            if (auction == null)
            {
                return NotFound();
            }
            if (!auction.IsActive)
            {
                ModelState.AddModelError(string.Empty, "This auction has expired.");
                return View(new CreateBidVm { AuctionId = id });
            }

            // Populate StartingBid with either the starting price or highest bid + 1
            var model = new CreateBidVm
            {
                AuctionId = id,
                StartingBid = auction.Bids.Any()
                    ? auction.GetHighestBid().Amount + 1  
                    : auction.StartingPrice               
            };

            return View(model);
        }
        
        // POST : AuctionsController/CreateBid
        [HttpPost]
        public ActionResult CreateBid(int id, CreateBidVm createBidVm)
        {
            try
            {
                Auction auction = _auctionService.GetById(id);

                if (auction == null)
                {
                    return NotFound(); 
                }
                
                if (!auction.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "This auction has expired.");
                    return View(createBidVm); // Return the view with the error message
                }

                if (ModelState.IsValid)
                {
                    double amount = createBidVm.Amount;
                    string userName = User.Identity.Name;

                    // Place the bid if all checks are valid
                    _auctionService.PlaceBid(id, amount, userName);
                    return RedirectToAction("Active");
                }

                return View(createBidVm);
            }
            catch (DataException e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while placing the bid. Please try again.");
                return View(createBidVm);
            }
        }

        
        
        // GET : AuctionsController/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Auction auction = _auctionService.GetById(id, User.Identity.Name);
            
            if (auction == null)
            {
                return NotFound(); 
            }
            
            var model = new EditAuctionVm
            {
                Description = auction.Description 
            };

            return View(model); 
        }

        
        // POST : AuctionsController/EditAuction/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditAuctionVm editAuctionVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Auction auction = _auctionService.GetById(id, User.Identity.Name);

                    if (auction == null)
                    {
                        return NotFound(); 
                    }
                    
                    auction.Description = editAuctionVm.Description;
                    
                    _auctionService.UpdateAuction(auction);
                    
                    return RedirectToAction("Details", new { id = auction.Id });
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again later.");
                }
            }
            
            return View(editAuctionVm);
        }
    }

}
