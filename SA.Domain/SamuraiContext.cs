using Microsoft.EntityFrameworkCore;
using SA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA.Data
{
    public class SamuraiContext:DbContext
    {
        public SamuraiContext(DbContextOptions<SamuraiContext> options):base(options)
        {

        }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<SamuraiBattle> SamuraiBattles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(k => new { k.BattleId,k.SamuraiId });
            //modelBuilder.Entity<Samurai>().Property<DateTime>("LastModified"); shadowproperty
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
                modelBuilder.Entity(entityType.Name).Ignore("IsDirty");
            }
            //modelBuilder.Entity<Samurai>().Property(p => p.SecretIdentity).IsRequired();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    "Data Source=DESKTOP-0GGJ5R6\\SQLEXPRESS;Initial Catalog=SamuraiDataCoreWeb;Integrated Security=True;", option => option.MaxBatchSize(30));
            //optionsBuilder.EnableSensitiveDataLogging();
            //base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries().Where(e=>e.State==EntityState.Added||e.State==EntityState.Modified))
            {
                entry.Property("LastModified").CurrentValue = DateTime.Now;
            }
            return base.SaveChanges();
        }
    }
}
