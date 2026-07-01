using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using p_freight.Data;
using p_freight.Models;

namespace p_freight.Controllers;

[Authorize(Roles = "Admin")]
public class OrganisationsController : Controller
{
    private readonly FreightDbContext _context;

    public OrganisationsController(FreightDbContext context)
    {
        _context = context;
    }

    // GET: Organisations
    public async Task<IActionResult> Index()
    {
        return View(await _context.Organisations.ToListAsync());
    }

    // GET: Organisations/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var organisation = await _context.Organisations
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (organisation == null) return NotFound();

        return View(organisation);
    }

    // GET: Organisations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Organisations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Country")] Organisation organisation)
    {
        if (ModelState.IsValid)
        {
            organisation.Id = Guid.NewGuid().ToString();
            organisation.CreatedAt = DateTime.Now;
            organisation.UpdatedAt = DateTime.Now;
            _context.Add(organisation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(organisation);
    }

    // GET: Organisations/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var organisation = await _context.Organisations.FindAsync(id);
        if (organisation == null) return NotFound();
        return View(organisation);
    }

    // POST: Organisations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Country,CreatedAt")] Organisation organisation)
    {
        if (id != organisation.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                organisation.UpdatedAt = DateTime.Now;
                _context.Update(organisation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganisationExists(organisation.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(organisation);
    }

    private bool OrganisationExists(string id)
    {
        return _context.Organisations.Any(e => e.Id == id);
    }
}
