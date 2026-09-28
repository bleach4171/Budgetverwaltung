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
    }
}

/* Quellen:

Microsoft Dokumentation Add a model to an ASP.NET Core MVC app
w3schools Seite zu C# Classes and Objects
w3schools Seite zu C# Properties (get und set)
YouTube Videos zum Thema ASP.NET Core MVC Model erstellen für Anfänger 
*/