using System.ComponentModel.DataAnnotations;
using Budgetverwaltung.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budgetverwaltung.ViewModels
{
    // Das ist mein ViewModel für das Formular einer Buchung
    // Hier packe ich alles rein was ich im Formular brauche
    public class TransactionFormViewModel
    {
        // Bei Edit brauche ich die Id der Buchung, bei Create bleibt sie 0
        public int Id { get; set; }

        // Der Betrag darf nicht leer sein und muss zwischen 0,01 und 999999,99 liegen
        [Required(ErrorMessage = "Bitte einen Betrag eingeben")]
        [Range(0.01, 999999.99, ErrorMessage = "Der Betrag muss zwischen 0,01 und 999999,99 liegen")]
        public decimal Amount { get; set; }

        // Das Datum darf nicht leer sein
        [Required(ErrorMessage = "Bitte ein Datum eingeben")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        // Der Typ muss INCOME oder EXPENSE sein, andere Zahlen sind nicht erlaubt
        [Required(ErrorMessage = "Bitte einen Typ auswählen")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Bitte einen gültigen Typ auswählen")]
        public TransactionType Type { get; set; } = TransactionType.EXPENSE;

        // Die Beschreibung darf leer bleiben und höchstens 200 Zeichen haben
        [StringLength(200, ErrorMessage = "Die Beschreibung darf höchstens 200 Zeichen haben")]
        public string? Description { get; set; }

        // Die Kategorie muss ausgewählt sein
        // Eine Id kleiner als 1 gibt es nicht, deshalb prüfe ich mit Range
        [Range(1, int.MaxValue, ErrorMessage = "Bitte eine Kategorie auswählen")]
        public int CategoryId { get; set; }

        // Das ist die Liste mit allen Kategorien für das Dropdown Menü im Formular
        // Die wird nicht vom Benutzer ausgefüllt, deshalb ValidateNever
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}

/* Quellen:

Technologiepfad C# ASP.NET Core MVC der Model Ausschnitt
Microsoft Dokumentation Model validation in ASP.NET Core MVC (DataAnnotations)
Microsoft Dokumentation SelectList and SelectListItem in ASP.NET Core
w3schools Seite zu C# Enums
w3schools Seite zu C# List Collection
*/