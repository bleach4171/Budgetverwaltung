namespace Budgetverwaltung.Models
{
    // Das ist meine Auswahl für den Typ einer Buchung
    // Es gibt nur diese zwei Möglichkeiten
    public enum TransactionType
    {
        // Das ist eine Einnahme
        INCOME = 0,

        // Das ist eine Ausgabe
        EXPENSE = 1
    }
}

/* Quellen:

Technologiepfad C# ASP.NET Core MVC
w3schools Seite zu C# Enums
Microsoft Dokumentation Enumeration types in C#
*/