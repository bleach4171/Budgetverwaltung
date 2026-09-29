using Budgetverwaltung.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budgetverwaltung.ViewModels
{
    // Das ist mein ViewModel für die Statistikseite
    // Es enthält die Filter und die berechneten Ergebnisse zusammen
    public class StatisticsViewModel
    {
        // Ab diesem Datum werden die Buchungen gezählt, leer heißt kein Anfang
        public DateTime? Von { get; set; }

        // Bis zu diesem Datum werden die Buchungen gezählt, leer heißt kein Ende
        public DateTime? Bis { get; set; }

        // Hier kann ich nach Einnahme oder Ausgabe filtern, leer heißt beides
        public TransactionType? Type { get; set; }

        // Hier kann ich nach einer Kategorie filtern, 0 heißt alle Kategorien
        public int CategoryId { get; set; }

        // Das ist die Liste mit allen Kategorien für das Dropdown Menü im Filter
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        // Ab hier kommen nur die Ergebnisse, die ich berechne und nicht der Benutzer eingibt

        // Die Summe von allen Einnahmen im gewählten Zeitraum
        public decimal SummeEinnahmen { get; set; }

        // Die Summe von allen Ausgaben im gewählten Zeitraum
        public decimal SummeAusgaben { get; set; }

        // Der Saldo ist Einnahmen minus Ausgaben
        public decimal Saldo => SummeEinnahmen - SummeAusgaben;

        // Der Name von der Kategorie mit den meisten Ausgaben, leer wenn es keine Ausgaben gibt
        public string? TopKategorieName { get; set; }

        // Die Summe von der Top Kategorie
        public decimal TopKategorieSumme { get; set; }

        // Hier steht für jede Kategorie, wie viel an Ausgaben zusammengekommen ist
        public List<KategorieSumme> AusgabenNachKategorie { get; set; } = new List<KategorieSumme>();

        // Hier steht für jeden Monat, wie viel Einnahmen und Ausgaben es gab
        public List<MonatsSumme> BuchungenNachMonat { get; set; } = new List<MonatsSumme>();

        // Das sagt mir, ob es im gewählten Zeitraum überhaupt Buchungen gibt
        public bool HatBuchungen { get; set; }
    }

    // Das ist eine kleine Klasse nur für die Gruppierung nach Kategorie
    public class KategorieSumme
    {
        public string Name { get; set; } = "";
        public decimal Summe { get; set; }
    }

    // Das ist eine kleine Klasse nur für die Gruppierung nach Monat
    public class MonatsSumme
    {
        // Zum Beispiel 2026-09, damit ich leicht sortieren kann
        public string Monat { get; set; } = "";
        public decimal Einnahmen { get; set; }
        public decimal Ausgaben { get; set; }
    }
}

/* Quellen:

Projektauftrag BudgetBook (Statistik, Gesamtübersicht)
Microsoft Dokumentation Model validation in ASP.NET Core MVC
Microsoft Dokumentation SelectList and SelectListItem in ASP.NET Core
w3schools Seite zu C# Nullable Types (die Fragezeichen bei DateTime und TransactionType)
w3schools Seite zu C# List Collection
*/