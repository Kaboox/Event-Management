using Event_Management.Data;
using Event_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Management.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        
        public EventsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // --- DOSTĘP PUBLICZNY (Dla każdego) ---

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(e => e.Category).Include(e => e.Place);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Place)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null) return NotFound();

            // --- Sprawdzamy, czy zalogowany user jest na liście ---
            bool isRegistered = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Sprawdzamy w tabeli Registrations
                    isRegistered = await _context.Registrations
                        .AnyAsync(r => r.EventId == id && r.UserId == user.Id);
                }
            }
            ViewBag.IsUserRegistered = isRegistered;
            // -------------------------------------------------------------

            return View(@event);
        }

        // --- METODA DLA ZALOGOWANYCH USERÓW (Zapisywanie się) ---

        [Authorize] // Wymaga logowania, ale nie wymaga bycia Adminem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge(); // Jeśli niezalogowany, wyślij do logowania

            // Sprawdzenie czy już się nie zapisał
            var existing = await _context.Registrations
                .FirstOrDefaultAsync(r => r.EventId == id && r.UserId == user.Id);

            if (existing == null)
            {
                var registration = new Registration
                {
                    EventId = id,
                    UserId = user.Id,
                    RegistrationDate = DateTime.Now
                };
                _context.Registrations.Add(registration);
                await _context.SaveChangesAsync();
            }

            // Powrót do szczegółów eventu
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // --- STREFA ADMINA (Create, Edit, Delete) ---

        // GET: Events/Create
        [Authorize(Roles = "Admin")] // Tylko Admin
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Tylko Admin
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Date,CategoryId,PlaceId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", @event.CategoryId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name", @event.PlaceId);
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin")] // Tylko Admin
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", @event.CategoryId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name", @event.PlaceId);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Tylko Admin
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,CategoryId,PlaceId")] Event @event)
        {
            if (id != @event.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", @event.CategoryId);
            ViewData["PlaceId"] = new SelectList(_context.Places, "Id", "Name", @event.PlaceId);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin")] // Tylko Admin
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Place)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Tylko Admin
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}