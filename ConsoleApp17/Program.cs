using System;
using SA.Model;
using SA.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ConsoleApp17
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            _context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            //InsertPkFkGraph();
            //InsertPkFkGraphMultipleChildren();
            //InsertNewOneToOneGraph();
            //AddChildToExistingObject();
            //AddOneToOneToExistingObjectWhileTracked();
            //AddBattles();
            //AddManyToMayWithFks();
            //EagerLoadingWithInclude();
            //EagerLoadManyToManyAkaChildrenGrandChildren();
            //EagerLoadingWithMultipleBranches();
            //Console.WriteLine("Hello World!");
            //AnonymousTypeViaPRojectionRelatedData();
            //EagerLoadingViaProjectionNotQuite();
            //EagerLoadingViaProjectionNope();
            //ExplicitLoad();
            //UsingRelatedDataForFiletrsAndMore();
            //AddGraphAllNew();
            //AddGraphWithKeys();
            //AttachGraphAllNew();
            //AttachGraphWithKeys();
            //UpdateGraphAllNew();
            //UpdateGraphWithKeys();
            AddGraphViaEntry();
            AddGrapViaEntryhWithKeys();
            Console.Read();
        }

        public static void AddGraphViaEntry()
        {
            var samuraiGraph = new Samurai { Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Entry(samuraiGraph).State = EntityState.Added;
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        public static void AddGrapViaEntryhWithKeys()
        {
            var samuraiGraph = new Samurai { Id = 1, Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Id = 1, Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Entry(samuraiGraph).State = EntityState.Added;
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }


        public static void DisplayEntries(List<EntityEntry> entries, string method)
        {
            Console.WriteLine($"Method Initiated {method}");
            foreach (var item in entries)
            {
                Console.WriteLine($"{item.Entity.GetType().Name}:{item.State.ToString()}");
            }
        }

        public static void UpdateGraphAllNew()
        {
            var samuraiGraph = new Samurai { Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Update(samuraiGraph);
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        public static void UpdateGraphWithKeys()
        {
            var samuraiGraph = new Samurai { Id = 1, Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Id = 1, Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Update(samuraiGraph);
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        public static void AddGraphAllNew()
        {
            var samuraiGraph = new Samurai { Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samuraiGraph);
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        public static void AddGraphWithKeys()
        {
            var samuraiGraph = new Samurai { Id = 1, Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Id = 1, Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samuraiGraph);
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        public static void AttachGraphAllNew()
        {
            var samuraiGraph = new Samurai { Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Attach(samuraiGraph);
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        public static void AttachGraphWithKeys()
        {
            var samuraiGraph = new Samurai { Id = 1, Name = "TexasWarrior", Quotes = new List<Quote>() { new Quote { Id = 1, Text = "This is new" } } };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Attach(samuraiGraph);
                var es = context.ChangeTracker.Entries().ToList();
                DisplayEntries(es, "AddGraphAllNew");
            }
        }

        private static void UsingRelatedDataForFiletrsAndMore()
        {
            var samurais = _context.Samurais.Where(s => s.Quotes.Any(x => x.Text.Contains("happy"))).ToList();
        }

        private static void ExplicitLoad()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            _context.Entry(samurai).Collection(x => x.Quotes).Query().Where(x => x.Text.Contains("happy")).Load();
            _context.Entry(samurai).Reference(x => x.SecretIdentity).Load();
        }
        private static void AnonymousTypeViaPRojectionRelatedData()
        {
            var samurais = _context.Samurais.Select(s => new { s.Id, s.SecretIdentity.RealName, QuotesCount = s.Quotes.Count }).ToList();
        }

        private static void RelatedObjectsFixUp()
        {
            var samurai = _context.Samurais.Find(1);
            var quotes = _context.Quotes.Where(x => x.Id == 1).ToList();
        }

        private static void EagerLoadingViaProjectionNope()
        {
            var samurais = _context.Samurais.Select(s => new { Samurai = s, Quotes = s.Quotes.Where(x => x.Text.Contains("happy")).ToList() }).ToList();
            //all results are in memory navigation is not fixed up
        }

        private static void EagerLoadingViaProjectionNotQuite()
        {
            var samurais = _context.Samurais.Select(s => new { Samurai = s, Quotes = s.Quotes }).ToList();
            //all results are in memory navigation is not fixed up
        }
        private static void EagerLoadingWithMultipleBranches()
        {
            var samurais = _context.Samurais.Include(x => x.SecretIdentity).Include(x => x.Quotes).ToList();
        }
        private static void EagerLoadingWithInclude()
        {
            var samuraisWithQuotes = _context.Samurais.Include(x => x.Quotes).ToList();
        }

        private static void EagerLoadManyToManyAkaChildrenGrandChildren()
        {
            var samuraiWithBattles = _context.Samurais.Include(x => x.SamuraiBattles)
                                                        .ThenInclude(sb => sb.Battle).ToList();
        }

        private static void AddBattles()
        {
            _context.Battles.AddRange(
                new Battle { Name = "Battle Of Shiroyama", StartDate = new DateTime(1977, 1, 23), EndDate = new DateTime(1978, 10, 12) },
                new Battle { Name = "Seige Of OSaka", StartDate = new DateTime(1987, 1, 23), EndDate = new DateTime(1989, 10, 12) },
                new Battle { Name = "Bosin War", StartDate = new DateTime(1977, 1, 23), EndDate = new DateTime(1978, 10, 12) }
                );
            _context.SaveChanges();
        }

        private static void AddManyToMayWithFks()
        {
            _context = new SamuraiContext();
            var sb = new SamuraiBattle { SamuraiId = 1, BattleId = 1 };
            _context.SamuraiBattles.Add(sb);
            _context.SaveChanges();
        }

        private static void AddOneToOneToExistingObjectWhileTracked()
        {
            var samurai = _context.Samurais.Where(x => x.SecretIdentity == null).FirstOrDefault();
            samurai.SecretIdentity = new SecretIdentity() { RealName = "Sampson" };
            _context.SaveChanges();

        }

        private static void AddChildToExistingObject()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote() { Text = "I bet you are happy, that i have saved you" });
            //_context.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertPkFkGraph()
        {
            var samurai = new Samurai()
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote> {
                new Quote(){ Text="History Reminds the king not soldiers" }
            }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertPkFkGraphMultipleChildren()
        {
            var samurai = new Samurai()
            {
                Name = "Kyuzo",
                Quotes = new List<Quote> {
                new Quote(){ Text="Watch out for my sharp sword" },
                new Quote(){ Text="I told,Watch out for my sharp sword" }
            }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewOneToOneGraph()
        {
            var samurai = new Samurai() { Name = "Suzuki" };
            samurai.SecretIdentity = new SecretIdentity() { RealName = "Julie" };
            _context.Add(samurai);
            _context.SaveChanges();
        }
    }
}