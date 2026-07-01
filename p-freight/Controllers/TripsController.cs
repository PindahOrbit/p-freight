using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using p_freight.Data;
using p_freight.Models;

namespace p_freight.Controllers;

[Authorize]
public class TripsController : Controller
{
    private readonly FreightDbContext _context;

    public TripsController(FreightDbContext context)
    {
        _context = context;
    }

    // GET: Trips
    public async Task<IActionResult> Index()
    {
        var trips = await _context.Trips
            .Include(t => t.Driver)
            .Include(t => t.Load)
            .Include(t => t.Truck)
            .Include(t => t.Organisation)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return View(trips);
    }

    // GET: Trips/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var trip = await _context.Trips
            .Include(t => t.Driver)
            .Include(t => t.Load)
            .Include(t => t.Truck)
            .Include(t => t.Organisation)
            .Include(t => t.TripLocations)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (trip == null) return NotFound();

        return View(trip);
    }

    // POST: Trips/AcceptBid
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptBid(string bidId)
    {
        var bid = await _context.LoadBids
            .Include(b => b.Load)
            .FirstOrDefaultAsync(b => b.Id == bidId);

        if (bid == null) return NotFound();

        // 1. Create the Trip record
        var trip = new Trip
        {
            Id = Guid.NewGuid().ToString(),
            LoadId = bid.LoadId,
            DriverId = bid.DriverId,
            TruckId = bid.TruckId ?? "",
            OrganisationId = bid.OrganisationId,
            AgreedPrice = bid.OfferAmount,
            Currency = bid.Currency,
            Status = "SCHEDULED",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        // 2. Update status of the Load and the Bid
        bid.Status = "ACCEPTED";
        bid.Load.Status = "ACCEPTED";

        _context.Trips.Add(trip);
        _context.Update(bid);
        _context.Update(bid.Load);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = trip.Id });
    }

    // POST: Trips/UpdateLocation
    [HttpPost]
    public async Task<IActionResult> UpdateLocation(string tripId, double lat, double lng)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip == null) return NotFound();

        var loc = new TripLocation
        {
            Id = Guid.NewGuid().ToString(),
            TripId = tripId,
            OrganisationId = trip.OrganisationId,
            Latitude = lat,
            Longitude = lng,
            RecordedAt = DateTime.Now
        };

        _context.TripLocations.Add(loc);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
