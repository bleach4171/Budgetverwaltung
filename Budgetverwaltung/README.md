# Budgetverwaltung - Akhmed Osmayev

Ein Block Schulprojekt für die Verwaltung von eigenen Einnahmen und Ausgaben.
Jede angemeldete Person sieht und bearbeitet nur ihre eigenen Buchungen.


## Was das Programm kann

1. Registrierung, Login und Logout mit ASP.NET Core Identity
2. Buchungen anlegen, anzeigen, bearbeiten und löschen (aber nur die eigenen)
3. Kategorien für Einnahmen und Ausgaben, mit Prüfung ob Kategorie und Typ zusammenpassen
4. Statistik mit Summen für Einnahmen, Ausgaben und Saldo
5. Top Kategorie bei den Ausgaben
6. Gruppierung der Ausgaben nach Kategorie und der Buchungen nach Monat
7. Filter nach Zeitraum, Typ und Kategorie


## Verwendete Methoden/Techniken

1. ASP.NET Core MVC mit C#
2. Entity Framework Core mit SQL Server LocalDB
3. ASP.NET Core Identity für Login und Registrierung
4. Bootstrap 5 für das Aussehen der Seiten


## Wie man das Projekt startet

1. Projekt mit Visual Studio öffnen (Datei Budgetverwaltung.sln)
2. In der Package Manager Console den Befehl Update-Database ausführen, damit die Datenbank mit allen Migrationen aufgebaut wird
3. Mit F5 starten
4. Im Browser registrieren und danach einfach über Meine Buchungen loslegen


## Datenmodell

Die drei wichtigsten Tabellen sind User (kommt von Identity), Category und Transaction.
Eine Transaction gehört immer genau zu einem User und zu einer Category.
Das genaue ER Diagramm liegt als Bild bei der Abgabe von Auftrag 1.


## Projektstruktur

1. Models: Category, Transaction, TransactionType
2. ViewModels: TransactionFormViewModel, StatisticsViewModel
3. Controllers: TransactionsController, StatisticsController
4. Views: je ein Ordner für Transactions und Statistics
5. Data: ApplicationDbContext und die Migrationen


## Sicherheit

Es liegen keine Passwörter oder Zugangsdaten im Code. Die Verbindung zur Datenbank läuft über Trusted_Connection, also über den Windows Benutzer, ohne gespeichertes Passwort.


## Quellen

1. Microsoft Dokumentation Get started with ASP.NET Core MVC
2. Microsoft Dokumentation Introduction to Identity on ASP.NET Core
3. Microsoft Dokumentation Migrations Overview in Entity Framework Core
4. Projektauftrag BudgetBook
5. Technologiepfad C# ASP.NET Core MVC