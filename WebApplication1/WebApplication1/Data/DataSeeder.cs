using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Helpers;
using SiteWebMultiSport.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteWebMultiSport.Data
{
    public static class DataSeeder
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            // Vérifiez si la base contient déjà des données
            if (context.Disciplines.Any()) return;

            // Ajout des disciplines
            var disciplines = new List<Discipline>
            {
                new Discipline { Nom = "Football" },
                new Discipline { Nom = "Tennis" },
                new Discipline { Nom = "Natation" }
            };
            context.Disciplines.AddRange(disciplines);
            context.SaveChanges();

            // Ajout d'adhérants
            var adherants = new List<Adherant>
            {
                new Adherant { Name = "Alice", Email = "alice@example.com", Phone="0764584715", DateNaissance = "12/01/2002", IsSubscribed = true, PasswordHash = PasswordHelper.HashPassword("password1") },
                new Adherant { Name = "Bob", Email = "bob@example.com", Phone="0764584715",DateNaissance = "12/01/2002", IsSubscribed = true, PasswordHash = PasswordHelper.HashPassword("password2") },
                new Adherant { Name = "Clara", Email = "clara@example.com", Phone="0764584715",DateNaissance = "12/01/2002", IsSubscribed = true, PasswordHash = PasswordHelper.HashPassword("password3") }
            };
            context.Adherants.AddRange(adherants);
            context.SaveChanges();

            //ajout d'un encadrant
            var encadrant = new Adherant
            {
                Name = "Jean",
                Email = "encadrant@example.com",
                DateNaissance = "12/01/2002",
                PasswordHash = PasswordHelper.HashPassword("encadrant123"),
                Phone = "0764584715",
                IsEncadrant = true,
                IsSubscribed = true
            };
            context.Adherants.Add(encadrant);
            context.SaveChanges();
            // Ajout des sections liées aux disciplines
            var sections = new List<Section>
            {
                new Section { Nom = "Football Compétition", DisciplineId = disciplines[0].Id, EncadrantId=encadrant.Id},
                new Section { Nom = "Tennis Loisir", DisciplineId = disciplines[1].Id,  EncadrantId=encadrant.Id },
                new Section { Nom = "Tennis Compétition", DisciplineId = disciplines[1].Id,  EncadrantId=encadrant.Id },
                new Section { Nom = "Natation Débutant", DisciplineId = disciplines[2].Id,  EncadrantId=encadrant.Id }
            };
            context.Sections.AddRange(sections);
            context.SaveChanges();

        

         

         

            // Ajout des créneaux liés aux sections
            var creneaux = new List<Creneau>
            {
                new Creneau { Jour = "Lundi", HeureDebut = new TimeSpan(18, 00, 00), HeureFin = new TimeSpan(19, 00, 00), Lieu = "Stade A", Capacite = 20, SectionId = sections[0].Id },
                new Creneau { Jour = "Mercredi", HeureDebut =new TimeSpan(19, 00, 00), HeureFin = new TimeSpan(20, 00, 00), Lieu = "Courts B", Capacite = 10, SectionId = sections[1].Id },
                new Creneau { Jour = "Vendredi", HeureDebut = new TimeSpan(17, 00, 00), HeureFin = new TimeSpan(18, 00, 00), Lieu = "Piscine C", Capacite = 15, SectionId = sections[3].Id }
            };
            context.Creneaux.AddRange(creneaux);
            context.SaveChanges();
            // Ajout des relations many-to-many (inscription des adhérents aux créneaux)
            creneaux[0].Adherants.Add(adherants[0]); // Alice -> Football Compétition
            creneaux[1].Adherants.Add(adherants[1]); // Bob -> Tennis Loisir
            creneaux[2].Adherants.Add(adherants[2]); // Clara -> Natation Débutant

            context.SaveChanges();

            // Ajout d'un admin 
            var admin = new Adherant
            {
                Name = "Admin",
                Email = "admin@example.com",
                DateNaissance = "12/01/2002",
                PasswordHash = PasswordHelper.HashPassword("admin123"),
                Phone = "0764584715",
                IsAdmin = true
            };
            context.Adherants.Add(admin);

       
          
            context.SaveChanges();
        }
    }
}

