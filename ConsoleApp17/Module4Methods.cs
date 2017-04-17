using System;
using SA.Model;
using SA.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ConsoleApp17
{
    internal static class Module4Methods
    {
        public static void RawSqlCommand()
        {
            using (var context = new SamuraiContext())
            {

                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var affected = context.Database.ExecuteSqlCommand("update samurais set name=REPLACE(Name,'Prince1','Prince2')");
                Console.WriteLine($"Affected Row {affected}");
            }
        }

        private static void RawSqlCommandWithOutput()
        {
            using (var context = new SamuraiContext())
            {

                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var procResult = new SqlParameter()
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Direction = System.Data.ParameterDirection.Output,
                    ParameterName = "@procResult",
                    Size = 50
                };
                context.Database.ExecuteSqlCommand("exec FindLongestName @procResult OUT", procResult);
                Console.WriteLine($"Longest Name {procResult.Value}");

            }
        }

        public static void RawSql()
        {
            using (var context = new SamuraiContext())
            {

                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                //var samurais = context.Samurais.FromSql("select * from samurais").OrderBy(x=>x.Name).Where(s=>s.Name.Contains("Prince")).ToList();
                //samurais.ForEach(s => Console.WriteLine(s.Name));
                var samurais = context.Samurais.FromSql("EXEC FilterSamuraiByNamePart {0}", "Prince").OrderBy(x => x.Name).ToList();
                samurais.ForEach(s => Console.WriteLine(s.Name));
            }
        }

        private static void NoSqlCallingCustomFunctions()
        {
            using (var context = new SamuraiContext())
            {

                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var samurais = context.Samurais.Select(x => new { newName = ReverseString(x.Name) }).ToList();
                samurais.ForEach(item => Console.Write(item.newName));
            }
        }

        private static string ReverseString(string str)
        {
            var res = str.AsEnumerable();
            return string.Concat(res.Reverse());
        }

        private static void DeleteSamurai()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var samurai = context.Samurais.Where(x => x.Name == "PrincePriyanjeet FamilyDasDas1").FirstOrDefault();
                context.Samurais.Remove(samurai);
                context.SaveChanges();
            }
        }

        private static void DeleteSamuraiWithState()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var samurai = context.Samurais.Where(x => x.Name == "Prince").FirstOrDefault();
                //context.Samurais.Remove(samurai);
                context.Entry(samurai).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void DeleteSamuraiWhileNotTracked()
        {
            Samurai samurai;
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                samurai = context.Samurais.Where(x => x.Name == "Prince").FirstOrDefault();
            }

            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                //var samurai = context.Samurais.Where(x => x.Name == "Prince").FirstOrDefault();
                context.Samurais.Remove(samurai);
                //context.Entry(samurai).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void InsertBattles()
        {
            using (var context = new SamuraiContext())
            {
                DateTime startDate = DateTime.Now.AddYears(-1);
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                context.Battles.Add(new Battle() { Name = "ClashOFTitans", StartDate = startDate, EndDate = new DateTime(2017, 2, 27) });
                context.SaveChanges();
            }
        }

        private static void UpdateBattles()
        {
            Battle battle;

            using (var context = new SamuraiContext())
            {

                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                battle = context.Battles.FirstOrDefault();

            }

            using (var context = new SamuraiContext())
            {

                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                battle.EndDate = new DateTime(2017, 3, 5);
                context.Battles.Update(battle);
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateSamuraiDisconnected()
        {
            Samurai samurai;
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                samurai = context.Samurais.Find(1);
                samurai.Name += "Das1";


            }

            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                context.Samurais.Update(samurai);
                context.SaveChanges();
            }
        }

        private static void MultipleOperations()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var samurai = context.Samurais.Find(1);
                samurai.Name += "Das";
                context.Samurais.Add(new Samurai() { Name = "Gudiya" });
                context.SaveChanges();
            }
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                //var samurais = context.Samurais.Where(x=>x.Name.Equals("Prince",StringComparison.OrdinalIgnoreCase)).ToList(); 
                //var samurai = context.Samurais.Where(x => x.Name=="Prince").ToList(); 
                //var samurai = context.Samurais.Where(x => x.Name == "Prince").FirstOrDefault();
                //var samurai = context.Samurais.FirstOrDefault(x => x.Name == "Prince");
                var samurais = context.Samurais.ToList();
                samurais.ForEach(item => item.Name += " Family");
                context.SaveChanges();
            }
        }

        private static void RetrieveAndUpdateSamurais()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                //var samurais = context.Samurais.Where(x=>x.Name.Equals("Prince",StringComparison.OrdinalIgnoreCase)).ToList(); 
                //var samurai = context.Samurais.Where(x => x.Name=="Prince").ToList(); 
                //var samurai = context.Samurais.Where(x => x.Name == "Prince").FirstOrDefault();
                //var samurai = context.Samurais.FirstOrDefault(x => x.Name == "Prince");
                var samurai = context.Samurais.Find(1);
                samurai.Name += "Priyanjeet";
                context.SaveChanges();
            }
        }

        private static void MoreQueries()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                //var samurais = context.Samurais.Where(x=>x.Name.Equals("Prince",StringComparison.OrdinalIgnoreCase)).ToList(); 
                //var samurai = context.Samurais.Where(x => x.Name=="Prince").ToList(); 
                //var samurai = context.Samurais.Where(x => x.Name == "Prince").FirstOrDefault();
                //var samurai = context.Samurais.FirstOrDefault(x => x.Name == "Prince");
                var samurai = context.Samurais.Find(1);
            }
        }

        private static void SimpleSamuraiQuery()
        {
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                var samurais = context.Samurais.ToList();
            }
        }

        private static void InsertMultipleSamurais()
        {
            var samurai1 = new Samurai() { Name = "Prasanjeet" };
            var samurai2 = new Samurai() { Name = "Sonu" };
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                context.Samurais.AddRange(new List<Samurai> { samurai1, samurai2 });
                context.SaveChanges();
            }
        }

        private static void InsertSamuraiData()
        {
            var samurai = new Samurai() { Name = "Prince" };
            using (var context = new SamuraiContext())
            {
                context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}
