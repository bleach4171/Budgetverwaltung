using Budgetverwaltung.Data;
using Budgetverwaltung.Models;
using Budgetverwaltung.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Hier zeige ich eine einzelne Buchung
        // GET Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Ich suche die Buchung, aber nur unter den eigenen
            var buchung = await EigeneBuchungHolen(id);

            // Wenn die Id fehlt, ungültig ist oder die Buchung jemand anderem gehört, gibt es einen 404 Fehler
            if (buchung == null)
            {
                return NotFound();
            }

            return View(buchung);
        }

        // Hier zeige ich das leere Formular für eine neue Buchung
        // GET Transactions/Create
        public async Task<IActionResult> Create()
        {
            var model = new TransactionFormViewModel();
            await KategorienLaden(model);
            return View(model);
        }

        // Hier kommt das ausgefüllte Formular an
        // POST Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionFormViewModel model)
        {
            // Wenn die einfachen Prüfungen schon passen, prüfe ich noch die Kategorie
            if (ModelState.IsValid)
            {
                await KategoriePruefen(model);
            }

            // Wenn irgendwas nicht passt, zeige ich das Formular mit den Fehlermeldungen nochmal
            if (!ModelState.IsValid)
            {
                await KategorienLaden(model);
                return View(model);
            }

            // Hier baue ich aus dem Formular eine neue Buchung
            // Die UserId nehme ich von der angemeldeten Person und nicht aus dem Formular
            var buchung = new Transaction
            {
                Amount = model.Amount,
                BookingDate = model.BookingDate,
                Type = model.Type,
                Description = model.Description,
                CategoryId = model.CategoryId,
                UserId = _userManager.GetUserId(User) ?? ""
            };

            _context.Transactions.Add(buchung);
            await _context.SaveChangesAsync();

            // Danach gehe ich zurück zur Liste
            return RedirectToAction(nameof(Index));
        }

        // Hier zeige ich das Formular mit den Daten von einer bestehenden Buchung
        // GET Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Ich suche die Buchung, aber nur unter den eigenen
            var buchung = await EigeneBuchungHolen(id);

            // Wenn die Id fehlt, ungültig ist oder die Buchung jemand anderem gehört, gibt es einen 404 Fehler
            if (buchung == null)
            {
                return NotFound();
            }

            // Hier kopiere ich die Daten von der Buchung in das ViewModel für das Formular
            var model = new TransactionFormViewModel
            {
                Id = buchung.Id,
                Amount = buchung.Amount,
                BookingDate = buchung.BookingDate,
                Type = buchung.Type,
                Description = buchung.Description,
                CategoryId = buchung.CategoryId
            };

            await KategorienLaden(model);
            return View(model);
        }

        // Hier kommt das geänderte Formular an
        // POST Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionFormViewModel model)
        {
            // Die Id aus der Adresse und die Id aus dem Formular müssen gleich sein
            if (id != model.Id)
            {
                return NotFound();
            }

            // Ich suche die Buchung nochmal, weil ich sie ändern will
            // Auch hier zählen nur die eigenen Buchungen
            var buchung = await EigeneBuchungHolen(id);

            if (buchung == null)
            {
                return NotFound();
            }

            // Wenn die einfachen Prüfungen schon passen, prüfe ich noch die Kategorie
            if (ModelState.IsValid)
            {
                await KategoriePruefen(model);
            }

            // Wenn irgendwas nicht passt, zeige ich das Formular mit den Fehlermeldungen nochmal
            if (!ModelState.IsValid)
            {
                await KategorienLaden(model);
                return View(model);
            }

            // Hier übernehme ich die neuen Werte in die Buchung
            // Die UserId ändere ich nicht, die Buchung bleibt bei der gleichen Person
            buchung.Amount = model.Amount;
            buchung.BookingDate = model.BookingDate;
            buchung.Type = model.Type;
            buchung.Description = model.Description;
            buchung.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();

            // Danach gehe ich zurück zur Liste
            return RedirectToAction(nameof(Index));
        }

        // Hier zeige ich die Bestätigungsseite, bevor eine Buchung gelöscht wird
        // GET Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Ich suche die Buchung, aber nur unter den eigenen
            var buchung = await EigeneBuchungHolen(id);

            // Wenn die Id fehlt, ungültig ist oder die Buchung jemand anderem gehört, gibt es einen 404 Fehler
            if (buchung == null)
            {
                return NotFound();
            }

            return View(buchung);
        }

        // Hier wird die Buchung wirklich gelöscht, aber erst nach der Bestätigung
        // POST Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Auch hier suche ich nur unter den eigenen Buchungen
            var buchung = await EigeneBuchungHolen(id);

            if (buchung == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(buchung);
            await _context.SaveChangesAsync();

            // Danach gehe ich zurück zur Liste
            return RedirectToAction(nameof(Index));
        }

        // Diese Methode sucht eine Buchung, aber nur wenn sie der angemeldeten Person gehört
        // Ich schreibe sie nur einmal, Details, Edit und Delete benutzen sie
        private async Task<Transaction?> EigeneBuchungHolen(int? id)
        {
            // Ohne Id kann ich nichts suchen
            if (id == null)
            {
                return null;
            }

            string? userId = _userManager.GetUserId(User);

            // Hier prüfe ich die Id und die Person gleichzeitig
            return await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        // Diese Methode prüft, ob die gewählte Kategorie zur Buchung passt
        // Ich schreibe sie nur einmal, Create und Edit benutzen sie beide
        private async Task KategoriePruefen(TransactionFormViewModel model)
        {
            // Hier suche ich die gewählte Kategorie, sie muss es geben und aktiv sein
            var kategorie = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == model.CategoryId && c.IsActive);

            if (kategorie == null)
            {
                // Die Kategorie gibt es nicht oder sie ist nicht mehr aktiv
                ModelState.AddModelError("CategoryId", "Bitte eine gültige Kategorie auswählen");
            }
            else if (kategorie.Type != model.Type)
            {
                // Hier prüfe ich die Regel: Typ von der Buchung und Typ von der Kategorie müssen gleich sein
                ModelState.AddModelError("CategoryId", "Die Kategorie passt nicht zum gewählten Typ");
            }
        }

        // Diese Methode füllt die Auswahlliste mit den aktiven Kategorien
        // Ich schreibe sie nur einmal, Create und Edit benutzen sie beide
        private async Task KategorienLaden(TransactionFormViewModel model)
        {
            var kategorien = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            model.Categories = kategorien
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name + " (" + c.Type + ")"
                })
                .ToList();
        }
    }
}

/* Quellen:

Projektauftrag BudgetBook (Geschäftsregeln und Bestätigungsseite vor Löschen)
Technologiepfad C# ASP.NET Core MVC (Sicherheitsmuster im Controller)
Microsoft Dokumentation Add a controller to an ASP.NET Core MVC app
Microsoft Dokumentation Model validation in ASP.NET Core MVC (ModelState)
Microsoft Dokumentation Prevent Cross Site Request Forgery attacks in ASP.NET Core (ValidateAntiForgeryToken)
Microsoft Dokumentation Handle errors in ASP.NET Core (NotFound)
Microsoft Dokumentation Tutorial Part 8 Add Delete to an ASP.NET Core MVC app
w3schools Seite zu C# LINQ (Where, OrderBy, Select)
w3schools Seite zu C# Methods
*/