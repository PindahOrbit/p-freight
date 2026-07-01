using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using p_freight.Areas.Identity.Data;
using p_freight.Data;
using p_freight.Models;

namespace p_freight.Controllers;

[Authorize]
public class LoadsController : Controller
{
    private readonly FreightDbContext _context;
    private readonly UserManager<p_freightUser> _userManager;
    private const string DefaultOrgId = "org-loadlink-default";

    public LoadsController(FreightDbContext context, UserManager<p_freightUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Loads
    public async Task<IActionResult> Index()
    {
        var loads = await _context.Loads
            .Include(l => l.Customer)
            .Include(l => l.Organisation)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
        return View(loads);
    }

    // GET: Loads/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var load = await _context.Loads
            .Include(l => l.Customer)
            .Include(l => l.Organisation)
            .Include(l => l.LoadBids)
                .ThenInclude(b => b.Driver)
            .Include(l => l.LoadBids)
                .ThenInclude(b => b.Truck)
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (load == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var myTrucks = await _context.Trucks
            .Where(t => t.OwnerId == user!.Id)
            .ToListAsync();
        ViewData["MyTrucks"] = new SelectList(myTrucks, "Id", "RegistrationNumber");
        ViewData["CurrentUserId"] = user!.Id;

        return View(load);
    }

    // GET: Loads/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Loads/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PickupAddress,PickupLat,PickupLng,DropoffAddress,DropoffLat,DropoffLng,CargoType,Weight,SpecialNotes")] Load load)
    {
        var user = await _userManager.GetUserAsync(User);

        load.Id = Guid.NewGuid().ToString();
        load.CustomerId = user!.Id;
        load.OrganisationId = string.IsNullOrEmpty(user.OrganisationId) ? DefaultOrgId : user.OrganisationId;
        load.Status = "PENDING";
        load.CreatedAt = DateTime.Now;
        load.UpdatedAt = DateTime.Now;

        _context.Add(load);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: Loads/PlaceBid
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceBid(string loadId, string truckId, double offerAmount)
    {
        var user = await _userManager.GetUserAsync(User);
        var load = await _context.Loads.FindAsync(loadId);
        if (load == null) return NotFound();

        var bid = new LoadBid
        {
            Id = Guid.NewGuid().ToString(),
            LoadId = loadId,
            DriverId = user!.Id,
            TruckId = truckId,
            OfferAmount = offerAmount,
            Status = "PENDING",
            OrganisationId = load.OrganisationId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.LoadBids.Add(bid);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = loadId });
    }
}
