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
public class TrucksController : Controller
{
    private readonly FreightDbContext _context;
    private readonly UserManager<p_freightUser> _userManager;
    private const string DefaultOrgId = "org-loadlink-default";

    public TrucksController(FreightDbContext context, UserManager<p_freightUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Trucks
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var trucks = await _context.Trucks
            .Include(t => t.Organisation)
            .Include(t => t.Owner)
            .Where(t => t.OwnerId == user!.Id)
            .ToListAsync();
        return View(trucks);
    }

    // GET: Trucks/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var truck = await _context.Trucks
            .Include(t => t.Organisation)
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (truck == null) return NotFound();

        return View(truck);
    }

    // GET: Trucks/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Trucks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TruckType,RegistrationNumber,CapacityWeight")] Truck truck)
    {
        var user = await _userManager.GetUserAsync(User);
        
        truck.Id = Guid.NewGuid().ToString();
        truck.OwnerId = user!.Id;
        truck.OrganisationId = string.IsNullOrEmpty(user.OrganisationId) ? DefaultOrgId : user.OrganisationId;
        truck.IsVerified = false;
        truck.CreatedAt = DateTime.Now;
        truck.UpdatedAt = DateTime.Now;

        _context.Add(truck);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Trucks/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var truck = await _context.Trucks.FindAsync(id);
        if (truck == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (truck.OwnerId != user!.Id) return Forbid();

        return View(truck);
    }

    // POST: Trucks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,TruckType,RegistrationNumber,CapacityWeight,CreatedAt")] Truck truck)
    {
        if (id != truck.Id) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var existing = await _context.Trucks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null || existing.OwnerId != user!.Id) return Forbid();

        truck.OwnerId = user.Id;
        truck.OrganisationId = existing.OrganisationId;
        truck.UpdatedAt = DateTime.Now;
        _context.Update(truck);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TruckExists(string id)
    {
        return _context.Trucks.Any(e => e.Id == id);
    }
}
