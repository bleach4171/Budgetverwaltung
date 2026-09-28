namespace Budgetverwaltung.Models
{
    // Das ist meine Klasse für eine Kategorie zum Beispiel Essen oder Gehalt
    public class Category
    {
        // Das ist der Primärschlüssel, die Datenbank zählt die Zahl selbst hoch
        public int Id { get; set; }

        // Hier steht der Name der Kategorie
        // Ich setze ihn am Anfang auf leeren Text damit Visual Studio keine Warnung zeigt
        public string Name { get; set; } = "";

        // Hier steht ob die Kategorie für Einnahmen (INCOME) oder für Ausgaben (EXPENSE) ist
        public TransactionType Type { get; set; }

        // Hier steht ob die Kategorie noch benutzt werden darf
        // Wenn es false ist, taucht sie später nicht mehr in der Auswahl auf
        public bool IsActive { get; set; } = true;
    }
}

/* Quellen:

Projektauftrag BudgetBook das Datenmodell
Microsoft Dokumentation Add a model to an ASP.NET Core MVC app
w3schools Seite zu C# Classes and Objects
w3schools Seite zu C# Enums
*/