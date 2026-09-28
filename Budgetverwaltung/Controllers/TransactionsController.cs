using Budgetverwaltung.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budgetverwaltung.Controllers
{
    // Mit Authorize dürfen nur angemeldete Personen auf diesen Controller
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Hier lasse ich mir den Datenbankkontext und den UserManager automatisch reingeben
        public TransactionsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Das ist die Liste mit allen eigenen Buchungen
        // GET Transactions oder GET Transactions/Index
        public async Task<IActionResult> Index()
        {
            // Hier hole ich mir die Id von der Person die gerade angemeldet ist
            string? userId = _userManager.GetUserId(User);

            // Hier hole ich mir nur die Buchungen von dieser Person
            // Include holt mir gleich die passende Kategorie mit dazu
            var eigeneBuchungen = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.BookingDate)
                .ToListAsync();

            return View(eigeneBuchungen);
        }
    }
}

/* Quellen:

Technologiepfad C# ASP.NET Core MVC
Microsoft Dokumentation Filter methods, Authorize attribute in ASP.NET Core
Microsoft Dokumentation Loading Related Data in Entity Framework Core (Include)
Microsoft Dokumentation Introduction to Identity on ASP.NET Core (UserManager GetUserId)
w3schools Seite zu C# LINQ (Where und OrderBy)
*/