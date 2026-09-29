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
    // Mit Authorize dürfen nur angemeldete Personen auf die Statistik
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StatisticsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Hier zeige ich die Statistik mit den Filtern an
        // GET Statistics oder GET Statistics/Index?von=...&bis=...&type=...&categoryId=...
        public async Task<IActionResult> Index(DateTime? von, DateTime? bis, TransactionType? type, int categoryId = 0)
        {
            string? userId = _userManager.GetUserId(User);

            // Ich hole mir zuerst alle eigenen Buchungen
            var abfrage = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            // Jetzt wende ich die Filter der Reihe nach an, aber nur wenn sie gesetzt sind
            if (von.HasValue)
            {
                abfrage = abfrage.Where(t => t.BookingDate >= von.Value);
            }

            if (bis.HasValue)
            {
                abfrage = abfrage.Where(t => t.BookingDate <= bis.Value);
            }

            if (type.HasValue)
            {
                abfrage = abfrage.Where(t => t.Type == type.Value);
            }

            if (categoryId > 0)
            {
                abfrage = abfrage.Where(t => t.CategoryId == categoryId);
            }

            var buchungen = await abfrage.ToListAsync();

            // Ich baue das ViewModel und schreibe die Filter zurück rein
            // Damit stehen sie nach dem Absenden wieder im Formular
            var model = new StatisticsViewModel
            {
                Von = von,
                Bis = bis,
                Type = type,
                CategoryId = categoryId,
                HatBuchungen = buchungen.Any()
            };

            // Das ist der Randfall keine Daten vorhanden
            // Wenn die Liste leer ist, bleiben alle Summen einfach bei null also bei 0
            model.SummeEinnahmen = buchungen
                .Where(t => t.Type == TransactionType.INCOME)
                .Sum(t => t.Amount);

            model.SummeAusgaben = buchungen
                .Where(t => t.Type == TransactionType.EXPENSE)
                .Sum(t => t.Amount);

            // Hier gruppiere ich die Ausgaben nach Kategorie
            var ausgaben = buchungen.Where(t => t.Type == TransactionType.EXPENSE).ToList();

            model.AusgabenNachKategorie = ausgaben
                .GroupBy(t => t.Category != null ? t.Category.Name : "Ohne Kategorie")
                .Select(gruppe => new KategorieSumme
                {
                    Name = gruppe.Key,
                    Summe = gruppe.Sum(t => t.Amount)
                })
                .OrderByDescending(k => k.Summe)
                .ToList();

            // Die erste Zeile in der Liste ist automatisch die Top Kategorie, weil ich absteigend sortiert habe
            // Wenn es nur Einnahmen gibt, ist die Liste leer und TopKategorieName bleibt leer
            var topKategorie = model.AusgabenNachKategorie.FirstOrDefault();
            if (topKategorie != null)
            {
                model.TopKategorieName = topKategorie.Name;
                model.TopKategorieSumme = topKategorie.Summe;
            }

            // Hier gruppiere ich alle Buchungen nach Monat, damit ich Einnahmen und Ausgaben pro Monat sehe
            model.BuchungenNachMonat = buchungen
                .GroupBy(t => t.BookingDate.ToString("yyyy-MM"))
                .Select(gruppe => new MonatsSumme
                {
                    Monat = gruppe.Key,
                    Einnahmen = gruppe.Where(t => t.Type == TransactionType.INCOME).Sum(t => t.Amount),
                    Ausgaben = gruppe.Where(t => t.Type == TransactionType.EXPENSE).Sum(t => t.Amount)
                })
                .OrderBy(m => m.Monat)
                .ToList();

            // Zum Schluss lade ich noch die Kategorien für das Dropdown Menü vom Filter
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

            return View(model);
        }
    }
}

/* Quellen:

Projektauftrag BudgetBook (Statistik, Filter, Geschäftsregeln)
Technologiepfad C# ASP.NET Core MVC (Sicherheitsmuster im Controller)
Microsoft Dokumentation Add a controller to an ASP.NET Core MVC app
Microsoft Dokumentation LINQ GroupBy Method
w3schools Seite zu C# LINQ (Where, GroupBy, OrderBy, Sum)
Unterricht in Informatik (Aufgabenstellung Auftrag 4, Statistik, Gruppierung und Filter)
*/