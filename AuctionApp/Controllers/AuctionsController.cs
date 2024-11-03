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
            return View(auctionsVms);  // Pass the list of AuctionVm to the view
        }
        
        // GET: AuctionsController/Active
        public ActionResult Active()
        {
            // Get all active auctions from the auction service
            List<Auction> activeAuctions = _auctionService.GetAllActiveAuctions();
    
            // Map the Auction domain models to AuctionVm view models
            List<AuctionVm> activeAuctionVms = new List<AuctionVm>();
            foreach (var auction in activeAuctions)
            {
                activeAuctionVms.Add(AuctionVm.FromAuction(auction));
            }

            // Pass the list of active AuctionVm to the view
            return View(activeAuctionVms);
        }

        public ActionResult ByBid()
        {
            List<Auction> auctionsWithBid = _auctionService.GetByUserBid(User.Identity.Name);

            // Map the Auction domain models to AuctionVm view models
            List<AuctionVm> activeAuctionVms = new List<AuctionVm>();
            foreach (var auction in auctionsWithBid)
            {
                activeAuctionVms.Add(AuctionVm.FromAuction(auction));
            }

            // Pass the list of active AuctionVm to the view
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

            Auction auction = _auctionService.GetById(id); // current user
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
        
        
        // GET : AuctionsController/CreateBid
        public ActionResult CreateBid(int id)
        {
            // Fetch the auction by ID
            Auction auction = _auctionService.GetById(id);

            if (auction == null)
            {
                return NotFound();
            }

            // Check if the auction is active
            if (!auction.IsActive)
            {
                ModelState.AddModelError(string.Empty, "This auction has expired.");
                return View(new CreateBidVm { AuctionId = id });
            }

            // Populate StartingBid with the current bid or starting price
            var model = new CreateBidVm
            {
                AuctionId = id,
                StartingBid = Math.Max(auction.StartingPrice, auction.GetHighestBid()?.Amount ?? auction.StartingPrice) + 1
            };

            return View(model);
        }
        
        // POST : AuctionsController/CreateBid
        [HttpPost]
        public ActionResult CreateBid(int id, CreateBidVm createBidVm)
        {
            try
            {
                // Fetch the auction again to check its status
                Auction auction = _auctionService.GetById(id);

                if (auction == null)
                {
                    return NotFound(); // Return 404 if auction is not found
                }

                // Check if the auction is still active
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
                // Log the exception if necessary
                ModelState.AddModelError(string.Empty, "An error occurred while placing the bid. Please try again.");
                return View(createBidVm);
            }
        }

        
        
        // GET : AuctionsController/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Fetch the auction by ID and check if it belongs to the current user
            Auction auction = _auctionService.GetById(id, User.Identity.Name);
            
            if (auction == null)
            {
                return NotFound();  // Return 404 if the auction is not found or doesn't belong to the user
            }
            
            // Create the EditAuctionVm model and populate it with the current description
            var model = new EditAuctionVm
            {
                Description = auction.Description // Pre-fill the current description
            };

            return View(model); // Pass the model to the view
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
                    // Fetch the auction by ID to ensure it exists and belongs to the user
                    Auction auction = _auctionService.GetById(id, User.Identity.Name);

                    if (auction == null)
                    {
                        return NotFound(); // Return 404 if the auction is not found or doesn't belong to the user
                    }

                    // Update only the description of the auction
                    auction.Description = editAuctionVm.Description;

                    // Save the changes through the service layer
                    _auctionService.UpdateAuction(auction);

                    // Redirect to the auction details page after successful update
                    return RedirectToAction("Details", new { id = auction.Id });
                }
                catch (DataException)
                {
                    // Handle any data-related exceptions
                    ModelState.AddModelError("", "Unable to save changes. Try again later.");
                }
            }

            // If the model state is invalid, re-display the form with the current values
            return View(editAuctionVm);
        }

        // Other actions...
    }

}
