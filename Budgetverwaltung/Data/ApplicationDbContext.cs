using Budgetverwaltung.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budgetverwaltung.Data
{
    // Das ist mein Datenbankkontext, über ihn rede ich mit der Datenbank
    // Er erbt von IdentityDbContext, deshalb gibt es die Tabellen für Login und User schon
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Das ist die Tabelle für die Kategorien
        public DbSet<Category> Categories { get; set; }

        // Das ist die Tabelle für die Buchungen
        public DbSet<Transaction> Transactions { get; set; }

        // Diese Methode läuft, wenn die Datenbank aufgebaut wird
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Das muss ganz am Anfang stehen, sonst fehlen die Tabellen von Identity
            base.OnModelCreating(builder);

            // Hier lege ich die vier Standardkategorien an
            // Essen, Wohnen und Freizeit sind Ausgaben, Gehalt ist eine Einnahme
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Essen", Type = TransactionType.EXPENSE, IsActive = true },
                new Category { Id = 2, Name = "Wohnen", Type = TransactionType.EXPENSE, IsActive = true },
                new Category { Id = 3, Name = "Freizeit", Type = TransactionType.EXPENSE, IsActive = true },
                new Category { Id = 4, Name = "Gehalt", Type = TransactionType.INCOME, IsActive = true }
            );
        }
    }
}

/* Quellen:

Projektauftrag BudgetBook (Datenmodell)
Microsoft Dokumentation Data Seeding in Entity Framework Core (HasData)
Microsoft Dokumentation Introduction to Identity on ASP.NET Core
w3schools Seite zu C# Inheritance (Vererbung)
*/