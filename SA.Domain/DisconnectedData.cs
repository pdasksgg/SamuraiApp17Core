using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SA.Model;
namespace SA.Data
{
    public class DisconnectedData
    {
        SamuraiContext _context;

        /// <summary>
        /// _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        /// We are telling the context not bother tracking any objects
        /// that are returned from query,Since the context is short lived and disposed
        /// i.e per request, we are not going to get benifit
        /// from tracking,so there is no point to waste resource and time creating them
        /// We could have done through _context.Samurais.AsNoTracking().ToList(), but taking
        /// advantage of new feature.
        /// </summary>
        /// <param name="context"></param>
        public DisconnectedData(SamuraiContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public List<KeyValuePair<int, string>> GetSamuraiReferenceList()
        {
            var samurais = _context.Samurais.OrderBy(s => s.Name)
              .Select(s => new { s.Id, s.Name })
              .ToDictionary(t => t.Id, t => t.Name).ToList();
            return samurais;
        }


        public Samurai LoadSamuraiGraph(int id)
        {
            var samurai =
              _context.Samurais
              .Include(s => s.SecretIdentity)
              .Include(s => s.Quotes)
              .FirstOrDefault(s => s.Id == id);
            return samurai;
        }

        public void SaveSamuraiGraph(Samurai samurai)
        {
            _context.ChangeTracker.TrackGraph
              (samurai, e => ApplyStateUsingIsKeySet(e.Entry));
            _context.SaveChanges();
        }

        public void DeleteSamuraiGraph(int id)
        {
            //goal:  delete samurai , quotes and secret identity
            //       also delete any joins with battles
            //EF Core supports Cascade delete by convention
            //Even if full graph is not in memory, db is defined to delete
            //But always double check!
            var samurai = _context.Samurais.Find(id); //NOT TRACKING !!
            _context.Entry(samurai).State = EntityState.Deleted; //TRACKING
            _context.SaveChanges();
        }

        private static void ApplyStateUsingIsKeySet(EntityEntry entry)
        {
            //check if Key is already set for an entity
            //this helps to differentiate between new objects that needed to be added
            //and the old one that originated from db
            if (entry.IsKeySet)
            {
                //whatever api is sending graph into this method is responsible for setting IsDirty property
                //IsDirty notifies if the value is changed or not
                if (((ClientChangeTracker)entry.Entity).IsDirty)
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    entry.State = EntityState.Unchanged;
                }
            }
            else
            {
                entry.State = EntityState.Added;
            }
        }
    }
}
