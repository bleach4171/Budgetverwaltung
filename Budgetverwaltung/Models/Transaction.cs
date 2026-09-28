using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Budgetverwaltung.Models
{
    // Das ist meine Klasse für eine einzelne Buchung
    public class Transaction
    {
        // Das ist der Primärschlüssel, die Datenbank zählt die Zahl selbst hoch
        public int Id { get; set; }

        // Das ist der Betrag der Buchung, er muss zwischen 0,01 und 999999,99 liegen
        // Mit Column sage ich der Datenbank, dass es zwei Stellen nach dem Komma gibt
        [Range(0.01, 999999.99, ErrorMessage = "Der Betrag muss zwischen 0,01 und 999999,99 liegen")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Hier steht das Datum von der Buchung
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        // Hier steht INCOME für Einnahme oder EXPENSE für Ausgabe
        public TransactionType Type { get; set; }

        // Das ist eine kurze Beschreibung, sie darf leer bleiben und höchstens 200 Zeichen haben
        [StringLength(200, ErrorMessage = "Die Beschreibung darf höchstens 200 Zeichen haben")]
        public string? Description { get; set; }

        // Das ist der Fremdschlüssel, er zeigt auf die Id von der Category
        public int CategoryId { get; set; }

        // Damit kann ich später direkt auf die Kategorie von der Buchung zugreifen
        public Category? Category { get; set; }

        // Das ist der Fremdschlüssel, er zeigt auf die Id von der Person, der die Buchung gehört
        // Die Id von einem User ist bei Identity ein Text
        public string UserId { get; set; } = string.Empty;

        // Damit kann ich später direkt auf die Person von der Buchung zugreifen
        public IdentityUser? User { get; set; }
    }
}

/* Quellen:

Microsoft Dokumentation Add a model to an ASP.NET Core MVC app
Microsoft Dokumentation Relationships in Entity Framework Core (Foreign Keys)
w3schools Seite zu C# Data Types (decimal und DateTime)
w3schools Seite zu C# Properties (get und set)
StackOverflow Fragen zum Thema decimal Datentyp in Entity Framework 
*/